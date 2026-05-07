using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float lifeTime;

    private int damage;
    public bool usedByPlayer { get; private set; }

    public void Init(int damage, float velocity, bool usedByPlayer)
    {
        this.damage = damage;
        this.usedByPlayer = usedByPlayer;
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * velocity;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health enemy;
        if (usedByPlayer)
        {
            enemy = collision.gameObject.GetComponent<EnemyHealth>();
        }
        else
        {
            enemy = collision.gameObject.GetComponent<PlayerHealth>();
        }
        if (enemy != null)
        {
            enemy.TakeDamage(damage, GetComponent<Rigidbody2D>().linearVelocity.normalized);
            Destroy(gameObject);
        }
        else if (wallLayer == (wallLayer | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}
