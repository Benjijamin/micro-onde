using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaveFrontDrawer : MonoBehaviour
{
    [Serializable]
    private class Segment
    {
        public LineRenderer lineRenderer;
        public List<Transform> transforms;

        public void Clean()
        {
            lineRenderer.positionCount = 0;
        }
    }

    [SerializeField]
    List<Segment> usedToSegments = new List<Segment>();
    [SerializeField]
    List<Segment> usedFromSegments = new List<Segment>();

    Queue<Segment> toSegmentPool;
    Queue<Segment> fromSegmentPool;

    [SerializeField]
    private int poolSize = 50;

    [SerializeField]
    private float breakDistance = 2f;

    [SerializeField]
    private float drawFrequency = 1.0f;
    private float drawTimer = 0f;

    [SerializeField]
    LineRenderer toLineRendererPrefab;
    [SerializeField]
    LineRenderer fromLineRendererPrefab;

    void Start()
    {
        toSegmentPool = new Queue<Segment>();
        fromSegmentPool = new Queue<Segment>();

        for (int i = 0; i < poolSize; i++) 
        {
            toSegmentPool.Enqueue(new Segment
            {
                lineRenderer = Instantiate(toLineRendererPrefab, transform)
            });
            fromSegmentPool.Enqueue(new Segment
            {
                lineRenderer = Instantiate(fromLineRendererPrefab, transform)
            });
        }
    }

    void Update() 
    {
        drawTimer += Time.deltaTime;
    }

    public void DrawWaveFronts(List<EchoWave> waves) 
    {
        if (drawTimer > drawFrequency)
        {
            CleanSegments();

            foreach (EchoWave wave in waves)
            {
                DrawWaveFront(wave);
            }

            drawTimer = 0f;
        }
    }

    private void DrawWaveFront(EchoWave wave)
    {
        Vector3[] currentSegment = new Vector3[wave.Nodes.Count + 1];
        int segmentCount = 0;
        int bounceType = 0;

        for (int i = 0; i < wave.Nodes.Count; i++) 
        {
            EchoNode cn = wave.Nodes[i];
            EchoNode nn = wave.Nodes[(i + 1) % wave.Nodes.Count];

            //Remplacer par le collision break
            if (Vector3.Distance(cn.transform.position, nn.transform.position) > breakDistance) 
            {
                if (segmentCount != 0) 
                { 
                    AssignSegment(currentSegment, segmentCount, bounceType); 
                    segmentCount = 0; 
                }
                continue;
            }

            if (cn.CurrentBounces % 2 == nn.CurrentBounces % 2)
            {
                if (segmentCount == 0)
                {
                    currentSegment[segmentCount++] = cn.transform.position;
                }
                bounceType = cn.CurrentBounces % 2;
                currentSegment[segmentCount++] = nn.transform.position;
                if (nn == wave.Nodes[0])
                {
                    AssignSegment(currentSegment, segmentCount, bounceType);
                    segmentCount = 0;
                }
            }
            else 
            {
                if (segmentCount != 0) 
                { 
                    AssignSegment(currentSegment, segmentCount, bounceType); 
                }
                segmentCount = 0;
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
        foreach (Segment s in usedFromSegments)
        {
            s.Clean();
            fromSegmentPool.Enqueue(s);
        }
        usedFromSegments.Clear();
    }

    private void AssignSegment(Vector3[] points, int length, int bounceType) 
    {
        Segment s = bounceType == 0 ? GetToSegment() : GetFromSegment();

        if (s != null)
        {
            s.lineRenderer.positionCount = length;
            s.lineRenderer.SetPositions(points);
        }
        else
        {
            Debug.LogError("Not enough segments in the pool!");
        }
    }

    private Segment GetToSegment() 
    {
        Segment s = toSegmentPool.Dequeue();
        usedToSegments.Add(s);
        return s;
    }
    private Segment GetFromSegment() 
    {
        Segment s = fromSegmentPool.Dequeue();
        usedFromSegments.Add(s);
        return s;
    }
}
