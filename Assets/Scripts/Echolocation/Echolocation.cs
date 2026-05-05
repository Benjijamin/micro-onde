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
        if (Input.GetKeyDown(KeyCode.Space))
            EmitWave(transform.position, .5f, transform.up, 5, 4, 90, 60);


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

    public void EmitWave(Vector2 pos, float startRadius, Vector2 dir, float lifeTime, float speed, float angle, int nbNodes)
    {
        EchoWave wave = new EchoWave
        {
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

            node.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
            node.transform.rotation = Quaternion.Euler(0, 0, newAngle - 90);
            node.Speed = speed;
            node.MaxBounces = 5;
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
        for (int i = 0; i < echoWave.Nodes.Count; i++)
        {
            echoWave.Nodes[i].gameObject.SetActive(false);
            nodePool.Enqueue(echoWave.Nodes[i]);
        }
    }
}

public class EchoWave
{
    public List<EchoNode> Nodes;
    public float MaxlifeTime;
    public float CurrentlifeTime;
}