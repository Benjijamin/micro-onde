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

        public void Clean()
        {
            lineRenderer.positionCount = 0;
        }
    }

    [SerializeField]
    Queue<Segment> usedSegments = new Queue<Segment>();

    Queue<Segment> segmentPool;

    [SerializeField]
    private int poolSize = 50;

    [SerializeField]
    private float breakDistance = 2f;

    [SerializeField]
    private float drawFrequency = 1.0f;
    private float drawTimer = 0f;

    [SerializeField]
    LineRenderer lineRendererPrefab;

    void Start()
    {
        segmentPool = new Queue<Segment>();

        for (int i = 0; i < poolSize; i++) 
        {
            segmentPool.Enqueue(new Segment
            {
                lineRenderer = Instantiate(lineRendererPrefab, transform)
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
                bool[] connectedNodes = DrawWaveFront(wave);
                if (wave.Nodes.Count > 0)
                {
                    for (int i = wave.Nodes.Count - 1; i >= 0; i--)
                    {
                        if (!connectedNodes[i])
                        {
                            wave.Nodes[i].Expire();
                        }
                    }
                }
            }

            drawTimer = 0f;
        }
    }

    private bool[] DrawWaveFront(EchoWave wave)
    {
        bool[] connectedNodes = new bool[wave.Nodes.Count];
        Vector3[] currentSegment = new Vector3[wave.Nodes.Count + 1];
        int segmentCount = 0;

        for (int i = 0; i < wave.Nodes.Count; i++) 
        {
            EchoNode cn = wave.Nodes[i];
            EchoNode nn = wave.Nodes[(i + 1) % wave.Nodes.Count];

            //Remplacer par le collision break
            if (Vector3.Distance(cn.transform.position, nn.transform.position) > breakDistance)
            {
                if (segmentCount != 0)
                {
                    AssignSegment(currentSegment, segmentCount);
                    segmentCount = 0;
                }
                continue;
            }
            else
            {
                if (segmentCount == 0)
                {
                    currentSegment[segmentCount++] = cn.transform.position;
                    connectedNodes[i] = true;
                }
                currentSegment[segmentCount++] = nn.transform.position;
                connectedNodes[(i+1) % wave.Nodes.Count] = true;
                if (nn == wave.Nodes[0])
                {
                    AssignSegment(currentSegment, segmentCount);
                    segmentCount = 0;
                }
            }
        }

        return connectedNodes;
    }

    private void CleanSegments() 
    {
        while (usedSegments.Count > 0) 
        {
            Segment s = usedSegments.Dequeue();
            s.Clean();
            segmentPool.Enqueue(s);
        }
    }

    private void AssignSegment(Vector3[] points, int length) 
    {
        Segment s = GetSegment();

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

    private Segment GetSegment() 
    {
        if (segmentPool.Count > 0)
        {
            Segment s = segmentPool.Dequeue();
            usedSegments.Enqueue(s);
            return s;
        }
        else 
        {
            return null; 
        }
    }
}