using UnityEngine;

public class Melee : Weapon
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Vector2 swingBoxDimensions;    // x = length, y = width

    public override void Attack()
    {
        base.Attack();
        print("punch");
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.root.position + (transform.root.right * swingBoxDimensions.y), swingBoxDimensions, transform.root.rotation.z, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
