using UnityEngine;

public class Pistol : Gun
{
    protected override void Shoot()
    {
        base.Shoot();
        Bullet bullet = Instantiate(bulletprefab, transform.position, transform.rotation);
        bullet.Init(damage, bulletVelocity);
    }
}
