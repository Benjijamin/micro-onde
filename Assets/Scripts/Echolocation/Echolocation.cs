using AmplifyShaderEditor;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour
{
    [SerializeField] private GameObject EchoNodePrefab;
    [SerializeField] private GameObject EchoNodeParent;
    private List<EchoWave> waves = new List<EchoWave>();

    [SerializeField] private int poolSize = 300;
    Queue<EchoNode> nodePool;

    private WaveFrontDrawer drawer;

    private Dictionary<int, float> pingEvents = new Dictionary<int, float>();

    private int idCount = 0;

    public static Echolocation instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            nodePool = new Queue<EchoNode>();

            for (int i = 0; i < poolSize; i++)
            {
                EchoNode node = Instantiate(EchoNodePrefab, EchoNodeParent.transform).GetComponent<EchoNode>();
                nodePool.Enqueue(node);
            }
        }

        drawer = GetComponentInChildren<WaveFrontDrawer>();
    }

    private void Update()
    {
        for (int i = 0; i < waves.Count; ++i)
        {
            waves[i].CurrentlifeTime += Time.deltaTime;
            if (waves[i].CurrentlifeTime >= waves[i].MaxlifeTime)
            {
                RecycleWaveNodes(waves[i]);
                waves.RemoveAt(i);
            }
        }

        drawer.DrawWaveFronts(waves);
    }

    public void EmitWave(Vector2 pos, Vector2 dir, float speed, float angle, float startRadius, float lifeTime, int maxBounces,  int nbNodes)
    {
        EchoWave wave = new EchoWave
        {
            waveId = idCount++,
            Nodes = new List<EchoNode>(),
            MaxlifeTime = lifeTime
        };

        float angleStep = angle / (nbNodes - 1);
        float startAngle = -angle / 2f;

        for (int i = 0; i < nbNodes; i++)
        {
            float currentAngle = startAngle + (angleStep * i);

            Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            Vector2 direction = rotation * dir;

            Vector2 spawnPosition = pos + (direction * startRadius);

            float newAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            EchoNode node = GetNode();

            node.waveId = wave.waveId;
            node.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
            node.transform.rotation = Quaternion.Euler(0, 0, newAngle - 90);
            node.Speed = speed;
            node.MaxBounces = maxBounces;
            node.CurrentBounces = 0;
            node.gameObject.SetActive(true);
            wave.Nodes.Add(node);

            node.OnExpired += () =>
            {
                wave.Nodes.Remove(node);
                nodePool.Enqueue(node);
            };
        }

        waves.Add(wave);
    }

    EchoNode GetNode()
    {
        if (nodePool.Count > 0)
        {
            return nodePool.Dequeue();
        }

        return Instantiate(EchoNodePrefab, EchoNodeParent.transform).GetComponent<EchoNode>();
    }

    private void RecycleWaveNodes(EchoWave echoWave)
    {
        if (echoWave.Nodes.Count > 0) {
            for (int i = echoWave.Nodes.Count - 1; i >= 0; i--)
            {
                echoWave.Nodes[i].Expire();
            } 
        }
    }

    public void RegisterPing(EchoNode node) 
    {
        if (pingEvents.ContainsKey(node.waveId)) 
        {
            if (Time.time - pingEvents[node.waveId] < 3f)
            {
                ScoreManager.Instance.ScoreMultiScan();
            }
        }

        pingEvents[node.waveId] = Time.time;
    }
}

public class EchoWave
{
    public int waveId;
    public List<EchoNode> Nodes;
    public float MaxlifeTime;
    public float CurrentlifeTime;
}