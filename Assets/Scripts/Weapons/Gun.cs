using System;
using UnityEngine;

public class Gun : Weapon
{
    public Action<int, int> OnAttack;

    [SerializeField] protected Bullet bulletprefab;
    [SerializeField] protected int maxAmmoCount;
    [SerializeField] protected int ammoCount;
    [SerializeField] protected float bulletVelocity;

    public override void Attack()
    {
        base.Attack();
        ammoCount--;
        Shoot();
        OnAttack?.Invoke(ammoCount, maxAmmoCount);
    }

    protected virtual void Shoot()
    {
        print("pew");
    }

    public override bool CanAttack()
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
