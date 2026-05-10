using System;
using NaughtyAttributes;
using UnityEngine;

public class Gun : Weapon
{
    public Action<int, int> OnAttack;

    [SerializeField] protected Bullet bulletprefab;
    [SerializeField] protected GameObject muzzleFlashPrefab;
    [SerializeField] protected Transform muzzleTransform;
    [SerializeField] protected int maxAmmoCount;
    [SerializeField] protected int ammoCount;
    [SerializeField] protected float bulletVelocity;
    [SerializeField] protected CharacterAnim attackAnim;

    [Foldout("Audio")]
    [SerializeField] private bool canReload;
    [Foldout("Audio")]
    [ShowIf("canReload")]
    [SerializeField] private AudioClip reloadSound;
    [Foldout("Audio")]
    [ShowIf("canReload")]
    [SerializeField] private float reloadSoundDelay;

    public override void Attack(bool userIsPlayer)
    {
        base.Attack(userIsPlayer);
        if (userIsPlayer)
        {
            ammoCount--;
        }
        Shoot(userIsPlayer);
        OnAttack?.Invoke(ammoCount, maxAmmoCount);

        wielder.GetComponent<CharacterAnimationController>().SetAnimation(attackAnim);
        AudioManager.instance.Play(attackSound, AudioManager.instance.SFXVolume, false, true, transform.position);
    }

    protected virtual void Shoot(bool usedByPlayer)
    {
        animator.Play("Shoot", 0, 0);
        AudioManager.instance.Play(reloadSound, AudioManager.instance.SFXVolume, false, false, transform, reloadSoundDelay);

        if (muzzleFlashPrefab != null) 
        {
            if (muzzleTransform != null)
            {
                Instantiate(muzzleFlashPrefab, muzzleTransform.position, muzzleTransform.rotation);
            }
            else 
            {
                Debug.Log("You're stupid!");
            }
        }
    }

    public override bool CanAttack(Transform user)
    {
        return base.CanAttack() && ammoCount > 0;
    }

    public int GetMaxAmmoCount()
    {
        return maxAmmoCount;
    }

    public int GetAmmoCount()
    {
        return ammoCount;
    }
}
