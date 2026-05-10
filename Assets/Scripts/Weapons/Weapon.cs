using System;
using System.Collections;
using System.Diagnostics;
using NaughtyAttributes;
using Spine.Unity;
using UnityEngine;

public class Weapon : Interactable
{
    public static Action<Weapon> OnPickUp;

    public Transform wielder;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int damage;
    [SerializeField] protected Sprite heldSprite;
    [SerializeField] protected Sprite droppedSprite;
    [SerializeField] protected CharacterAnim characterPose;

    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    private bool canAttack;

    [Foldout("Audio")]
    [SerializeField] protected AudioClip attackSound;
    [Foldout("Audio")]
    [SerializeField] protected AudioClip readySound;

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

    protected IEnumerator AttackCooldown()
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
        spriteRenderer.sortingOrder = 2;

        wielder.GetComponent<CharacterAnimationController>().SetAnimation(characterPose);
        AudioManager.instance.Play(readySound, AudioManager.instance.SFXVolume, false, false, transform.position);
    }

    public virtual void Drop()
    {
        GetComponent<CircleCollider2D>().enabled = true;
        wielder = null;
        animator = null;
        spriteRenderer.sprite = droppedSprite;
        spriteRenderer.sortingOrder = 0;
    }

    public float GetAttackCooldown()
    {
        return attackCooldown;
    }
}
