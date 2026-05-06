using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreMultipliers", menuName = "Scriptable Objects/ScoreMultipliers")]
public class ScoreMultipliers : ScriptableObject
{
    [InfoBox("Kills statrt at 1, additive with itself, multiplicative with other bonuses. \n" +
        "Speedup est la vitesse que la duration descends selon le nombre de multis stacked")]
    public float killMultiSpeedup = 0.1f;

    public int killScore = 500;
    public float killMultiplier = 1f;
    public float killDuration = 3f;

    [InfoBox("Bonuses start at 1, additive with themselves, multiplicative with kill multi. \n" +
        "Speedup est la vitesse que la duration descends selon le nombre de multis stacked")]
    public float bonusMultiSpeedup = 0.02f;

    public int meleeScore = 300;
    public float meleeMultiplier = 1f;
    public float meleeDuration = 5f;

    [Space]
    public int dodgeScore = 100;
    public float dodgeMultiplier = 0.5f;
    public float dodgeDuration = 1f;

    [Space]
    public int gunSwapScore = 0;
    public float gunSwapMultiplier = 0.2f;
    public float gunSwapDuration = 3f;

    [Space]
    public int stealthKillScore = 200;
    public float stealthKillMultiplier = 0.5f;
    public float stealthKillDuration = 5f;

    [Space]
    public int blindKillScore = 500;
    public float blindKillMultiplier = 3f;
    public float blindKillDuration = 1f;

    [Space]
    public int multiScanScore = 100;
    public float multiScanMultiplier = 0.3f;
    public float multiScanDuration = 2f;

    [Space]
    public int lastBulletScore = 50;
    public float lastBulletMultiplier = 0.1f;
    public float lastBulletDuration = 2f;
}
