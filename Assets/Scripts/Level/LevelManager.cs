using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float respawnDelay;

    private GameObject[] enemies;
    private int enemyCount;
    private int killCount;

    private GameObject player;

    public Action OnLevelCleared;

    [Foldout("Audio")]
    [SerializeField] private AudioClip inGameTheme;

    private AudioPlayer inGameThemePlayer;

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

    private void Start()
    {
        inGameThemePlayer = AudioManager.instance.Play(inGameTheme, AudioType.Music, true);
        inGameThemePlayer.FadeIn(1, 0.5f);
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
        ScoreManager.Instance.ShowEndOfLevelScore(3f);
        OnLevelCleared?.Invoke();
    }

    private void OnDestroy()
    {
        inGameThemePlayer.FadeOut(0.5f);
    }
}
