using UnityEngine;

public class EnemyHealth : Health
{
    protected override void Die()
    {
        Destroy(gameObject);
    }
}
