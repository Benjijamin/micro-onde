using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Segment
{
    public LineRenderer lineRenderer;

    public void Clean() 
    {
        lineRenderer.positionCount = 0;
    }
}

public class WaveFrontDrawer : MonoBehaviour
{
    Queue<Segment> toSegmentPool;
    Queue<Segment> innerSegmentPool;
    Queue<Segment> fromSegmentPool;

    [SerializeField]
    List<Segment> usedToSegments = new List<Segment>();
    [SerializeField]
    List<Segment> usedInnerSegments = new List<Segment>();
    [SerializeField]
    List<Segment> usedFromSegments = new List<Segment>();

    [SerializeField]
    int poolSize = 50;

    [SerializeField]
    LineRenderer toLineRendererPrefab;
    [SerializeField]
    LineRenderer innerLineRendererPrefab;
    [SerializeField]
    LineRenderer fromLineRendererPrefab;

    void Start()
    {
        toSegmentPool = new Queue<Segment>();
        innerSegmentPool = new Queue<Segment>();
        fromSegmentPool = new Queue<Segment>();

        for (int i = 0; i < poolSize; i++) 
        {
            toSegmentPool.Enqueue(new Segment
            {
                lineRenderer = Instantiate(toLineRendererPrefab, transform)
            });
            innerSegmentPool.Enqueue(new Segment
            {
                lineRenderer = Instantiate(innerLineRendererPrefab, transform)
            });
            fromSegmentPool.Enqueue(new Segment
            {
                lineRenderer = Instantiate(fromLineRendererPrefab, transform)
            });
        }
    }

    [SerializeField]
    List<Transform> points;
    [SerializeField]
    List<int> bounces;

    [ContextMenu("Test")]
    public void Test() 
    {
        List<(Transform, int)> ppp = new List<(Transform, int)>();
        for(int i = 0; i < points.Count; i++) 
        {
            ppp.Add((points[i], bounces[i]));
        }

        DrawWaveFront(ppp);
    }

    public void DrawWaveFront(List<(Transform, int)> points)
    {
        CleanSegments();

        List<Vector3> currentSegment = new List<Vector3>();
        int bounceType = 0;

        for (int i = 0; i < points.Count; i++) 
        {
            (Transform, int) cp = points[i];
            (Transform, int) np = points[(i + 1) % points.Count];

            if (cp.Item2 == np.Item2)
            {
                if (currentSegment.Count == 0) currentSegment.Add(cp.Item1.position);
                bounceType = cp.Item2;
                currentSegment.Add(np.Item1.position);
                if (np == points[0]) AssignSegment(currentSegment, bounceType);
            }
            else 
            {
                if (currentSegment.Count != 0) AssignSegment(currentSegment, bounceType);
                currentSegment.Clear();
                currentSegment.Add(cp.Item1.position);
                currentSegment.Add(np.Item1.position);
                AssignSegment(currentSegment, -1);
                currentSegment.Clear();
            }
        }
    }

    private void CleanSegments() 
    {
        foreach (Segment s in usedToSegments)
        {
            s.Clean();
            toSegmentPool.Enqueue(s);
        }
        usedToSegments.Clear();
        foreach (Segment s in usedInnerSegments)
        {
            s.Clean();
            innerSegmentPool.Enqueue(s);
        }
        usedInnerSegments.Clear();
        foreach (Segment s in usedFromSegments)
        {
            s.Clean();
            fromSegmentPool.Enqueue(s);
        }
        usedFromSegments.Clear();
    }

    private void AssignSegment(List<Vector3> points, int bounceType) 
    {
        Segment s;

        switch (bounceType) 
        {
            case -1:
                s = GetInnerSegment();
                if (s != null)
                {
                    s.lineRenderer.positionCount = points.Count;
                    s.lineRenderer.SetPositions(points.ToArray());
                }
                else 
                {
                    Debug.LogError("Not enough segments in the pool!");
                }
                break;
            case 0:
                s = GetToSegment();
                if (s != null) 
                {
                    s.lineRenderer.positionCount = points.Count;
                    s.lineRenderer.SetPositions(points.ToArray());
                }
                else
                {
                    Debug.LogError("Not enough segments in the pool!");
                }
        break;
            case 1:
                s = GetFromSegment();
                if (s != null) {
                    s.lineRenderer.positionCount = points.Count;
                    s.lineRenderer.SetPositions(points.ToArray());
                }
                else
                {
                    Debug.LogError("Not enough segments in the pool!");
                }
        break;
            default: break;
        }
    }

    private Segment GetToSegment() 
    {
        Segment s = toSegmentPool.Dequeue();
        usedToSegments.Add(s);
        return s;
    }

    private Segment GetInnerSegment() 
    {
        Segment s = innerSegmentPool.Dequeue();
        usedInnerSegments.Add(s);
        return s;
    }

    private Segment GetFromSegment() 
    {
        Segment s = fromSegmentPool.Dequeue();
        usedFromSegments.Add(s);
        return s;
    }
}
