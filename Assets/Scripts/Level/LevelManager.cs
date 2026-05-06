using System;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject[] enemies;
    private int enemyCount;
    private int killCount;

    private GameObject player;

    public Action OnLevelCleared;

    public static LevelManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            enemies = GameObject.FindGameObjectsWithTag("enemy");
            player = GameObject.FindGameObjectsWithTag("Player").First();
            enemyCount = enemies.Length;

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Health>().onDeath += OnEnemyDeath;
            }

            player.GetComponent<Health>().onDeath += OnPlayerDeath;
        }
    }

    private void OnEnemyDeath()
    {
        killCount++;
        if (killCount >= enemyCount)
            ClearLevel();

    }

    private void OnPlayerDeath()
    {
        GameManager.instance.ReloadLevel();
    }

    private void ClearLevel()
    {
        OnLevelCleared?.Invoke();
    }
}
