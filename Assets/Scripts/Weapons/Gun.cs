using System;
using UnityEngine;

public class Gun : Weapon
{
    public Action<int, int> OnAttack;

    [SerializeField] protected Bullet bulletprefab;
    [SerializeField] protected int maxAmmoCount;
    [SerializeField] protected int ammoCount;
    [SerializeField] protected float bulletVelocity;

    public override void Attack(bool userIsPlayer)
    {
        base.Attack(userIsPlayer);
        if (userIsPlayer)
        {
            ammoCount--;
        }
        Shoot(userIsPlayer);
        OnAttack?.Invoke(ammoCount, maxAmmoCount);
    }

    protected virtual void Shoot(bool usedByPlayer)
    {

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
