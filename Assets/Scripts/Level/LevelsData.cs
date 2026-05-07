using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Scriptable Objects/LevelsData")]
public class LevelsData : ScriptableObject
{
    public List<LevelData> Levels;
}

[Serializable]
public struct LevelData
{
    [Scene] public string Scene;

    public string IntroText;
    public float IntroDuration;
}