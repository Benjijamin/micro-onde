using System;
using System.Collections;
using UnityEngine;

public class Weapon : Interactable
{
    public static Action<Weapon> OnPickUp;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int damage;

    private bool canAttack;

    private void Start()
    {
        canAttack = true;
    }

    public virtual void Attack(bool userIsPlayer)
    {
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public virtual bool CanAttack(Transform user = null)
    {
        return canAttack;
    }

    public override void Interact()
    {
        base.Interact();
        OnPickUp?.Invoke(this);
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public virtual void Drop()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
