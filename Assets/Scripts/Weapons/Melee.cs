using NaughtyAttributes;
using UnityEngine;

public class Melee : Weapon
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 swingBoxDimensions;    // x = hit wideness, y = hit range

    [Foldout("Audio")]
    [SerializeField] private AudioClip attackMissSound;

    public override void Attack(bool userIsPlayer)
    {
        base.Attack(userIsPlayer);
        Animate();
        float angle = Vector2.SignedAngle(Vector2.up, -transform.root.up);
        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.root.position + (-transform.root.up * swingBoxDimensions.y), swingBoxDimensions, angle, userIsPlayer ? enemyLayer : playerLayer);
        foreach (Collider2D enemy in enemies)
        {
            Vector2 direction = (enemy.transform.position - wielder.position).normalized;
            if (userIsPlayer)
            {
                bool swap = wielder.GetComponent<WeaponHolster>().HasSwappedRecently;
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage, direction, swap, true);
            }
            else
            {
                enemy.GetComponentInParent<PlayerHealth>().TakeDamage(damage, direction);
            }
        }
        AudioManager.instance.Play(enemies.Length > 0 ? attackSound : attackMissSound, AudioManager.instance.SFXVolume, false, true, transform.position);
    }

    public virtual void Animate()
    {
        animator.Play("Swing", 0, 0);
    }

    public override bool CanAttack(Transform user)
    {
        if(user != PlayerMovement.Instance.transform)
        {
            float distanceToPlayer = (user.transform.position - PlayerMovement.Instance.transform.position).magnitude;
            return base.CanAttack() && distanceToPlayer < swingBoxDimensions.x;
        }
        return base.CanAttack();
    }
}
