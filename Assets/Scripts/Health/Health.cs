using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected List<Transform> bloodPrefabs = new List<Transform>();
    [SerializeField] protected Transform bloodDeathPrefab;
    [MinMaxSlider(0, 5)][SerializeField] protected Vector2 bloodSplatDistance;

    public Action onDeath;

    protected bool isDead;

    [Foldout("Audio")]
    [SerializeField] private AudioClip[] deathSounds;

    public virtual void TakeDamage(int damage, Vector3 direction, bool recentSwap = false, bool melee = false)
    {
        if (isDead) { return; }
        health -= damage;

        if (health <= 0)
        {
            Die(recentSwap, melee);
            return;
        }
        Transform blood = Instantiate(bloodPrefabs[Random.Range(0, bloodPrefabs.Count)], transform.position + (direction * Random.Range(bloodSplatDistance.x, bloodSplatDistance.y)), Quaternion.identity);
        blood.up = direction;
    }

    protected virtual void Die(bool recentSwap = false, bool melee = false)
    {
        isDead = true;
        Instantiate(bloodDeathPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
        onDeath?.Invoke();

        AudioManager.instance.Play(deathSounds[Random.Range(0, deathSounds.Length)], AudioManager.instance.SFXVolume, false, true, transform.position);
    }
}
