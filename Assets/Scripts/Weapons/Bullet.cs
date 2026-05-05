using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float lifeTime;

    private int damage;

    public void Init(int damage, float velocity)
    {
        this.damage = damage;
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocity;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (wallLayer == (wallLayer | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
