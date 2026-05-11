using UnityEngine;

public class EnemyHealth : Health
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    protected override void Die(bool recentSwap = false, bool melee = false)
    {
        ScoreManager.Instance.ScoreKill(recentSwap, melee, !enemy.hasBeenAlerted, !enemy.hasBeenPinged, enemy.transform.position);

        enemy.DropWeapon();
        Explode();
        Destroy(enemy.gameObject);

        base.Die();
    }

    public override void TakeDamage(int damage, Vector3 direction, bool recentSwap = false, bool melee = false)
    {
        base.TakeDamage(damage, direction, recentSwap, melee);
    }

    private void Explode()
    {
        Gibs g = GetComponentInChildren<Gibs>();
        if (g != null)
        {
            g.Explode();
        }
    }
}
