using UnityEngine;

[CreateAssetMenu(fileName = "EchoEmitterSettings", menuName = "Scriptable Objects/EchoEmitterSettings")]
public class EchoEmitterSettings : ScriptableObject
{
    [Header("Wave Emission")]
    public int NbWaves;
    public float TimeInterval;

    [Header("Wave")]
    public float Speed;
    public float ConeAngle;
    public float Radius;
    
    [Header("Expiration")]
    public float LifeTime;
    public int MaxBounces;

    [Header("Resolution")]
    public int NbNodePerWave;
}