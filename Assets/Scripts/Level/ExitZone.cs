using System;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    [SerializeField] private GameObject canExitIndicator;
    private Collider2D col;

    private void Awake()
    {
        canExitIndicator.gameObject.SetActive(false);
        col = GetComponent<Collider2D>();
        col.enabled = false;
    }

    private void Start()
    {
        LevelManager.instance.OnLevelCleared += OnLevelCleared;
    }

    private void OnLevelCleared()
    {
        canExitIndicator.gameObject.SetActive(true);
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
        if (player != null)
        {
            GameManager.instance.NextLevel();
        }
    }
}
