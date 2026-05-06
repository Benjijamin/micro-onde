using UnityEngine;

public class EnemyHealth : Health
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    protected override void Die()
    {
        enemy.DropWeapon();
        Destroy(enemy.gameObject);

        base.Die();
    }
}
