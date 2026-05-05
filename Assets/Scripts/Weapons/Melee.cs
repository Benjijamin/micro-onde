using UnityEngine;

public class Melee : Weapon
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 swingBoxDimensions;    // x = length, y = width

    public override void Attack(bool userIsPlayer)
    {
        base.Attack(userIsPlayer);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.root.position + (transform.root.right * swingBoxDimensions.y), swingBoxDimensions, transform.root.rotation.z, userIsPlayer ? enemyLayer : playerLayer);
        foreach (Collider2D enemy in enemies)
        {
            if (userIsPlayer)
            {
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
            else
            {
                enemy.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    public override bool CanAttack(Transform user)
    {
        if(user != PlayerMovement.Instance.transform)
        {
            float distanceToPlayer = (user.transform.position - PlayerMovement.Instance.transform.position).magnitude;
            return base.CanAttack() && distanceToPlayer < swingBoxDimensions.y;
        }
        return base.CanAttack();
    }
}
