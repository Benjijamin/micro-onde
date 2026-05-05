using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int health;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
