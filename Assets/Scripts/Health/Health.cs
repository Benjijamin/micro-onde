using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected List<Transform> bloodPrefabs = new List<Transform>();
    [MinMaxSlider(0, 5)][SerializeField] protected Vector2 bloodSplatDistance;

    public virtual void TakeDamage(int damage, Vector3 direction)
    {
        health -= damage;

        Transform blood = Instantiate(bloodPrefabs[Random.Range(0, bloodPrefabs.Count)], transform.position + (direction * Random.Range(bloodSplatDistance.x, bloodSplatDistance.y)), Quaternion.identity);
        blood.up = direction;

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
