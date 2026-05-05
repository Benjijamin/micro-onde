using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Echolocation : MonoBehaviour
{
    [SerializeField] private GameObject EchoNodePrefab;
    private List<EchoWave> waves = new List<EchoWave>();

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
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EmitWave(transform.position, .5f, transform.up, 5, 2, 90, 60);
    }

    public void EmitWave(Vector2 pos, float startRadius, Vector2 dir, float lifeTime, float speed, float angle, int nbNodes)
    {
        EchoWave wave = new EchoWave
        {
            Nodes = new List<EchoNode>(),
            MaxlifeTime = lifeTime,
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

            EchoNode newNode = Instantiate(EchoNodePrefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.Euler(0, 0, newAngle - 90)).GetComponent<EchoNode>();
            newNode.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
            newNode.Speed = speed;
            newNode.MaxBounces = 5;
            wave.Nodes.Add(newNode);
        }

        waves.Add(wave);
    }
}

public struct EchoWave
{
    public List<EchoNode> Nodes;
    public float MaxlifeTime;
    public float CurrentlifeTime;
}