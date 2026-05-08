using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float respawnDelay;

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

            player.GetComponent<PlayerHealth>().OnPlayerDeath += OnPlayerDeath;
        }
    }

    private void OnEnemyDeath()
    {
        killCount++;
        if (killCount >= enemyCount)
            ClearLevel();

    }

    private void OnPlayerDeath(bool isSuicide)
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(RestartLevel(isSuicide));
    }

    private IEnumerator RestartLevel(bool isSuicide)
    {
        yield return new WaitForSeconds(respawnDelay);
        LevelLoader.instance.ReloadLevel();
        ScoreManager.Instance.RevertScore();
    }

    private void ClearLevel()
    {
        MessageManager.instance.ShowLevelCleared(3);
        StartCoroutine(ScoreManager.Instance.ShowEndOfLevelScore(3f));
        OnLevelCleared?.Invoke();
        ScoreManager.Instance.RecordScore();
    }
}
