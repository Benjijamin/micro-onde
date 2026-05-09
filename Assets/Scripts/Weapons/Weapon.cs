using System;
using System.Collections;
using UnityEngine;

public class Weapon : Interactable
{
    public static Action<Weapon> OnPickUp;

    public Transform wielder;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int damage;
    [SerializeField] protected Sprite heldSprite;
    [SerializeField] protected Sprite droppedSprite;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private bool canAttack;

    private void Start()
    {
        canAttack = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponentInParent<Animator>();
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
        animator = GetComponentInParent<Animator>();
        spriteRenderer.sprite = heldSprite;
    }

    public virtual void Drop()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        wielder = null;
        animator = null;
        spriteRenderer.sprite = droppedSprite;
    }

    public float GetAttackCooldown()
    {
        return attackCooldown;
    }
}
