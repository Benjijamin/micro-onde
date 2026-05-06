using UnityEngine;

public class SingleShotGun : Gun
{
    protected override void Shoot(bool usedByPlayer)
    {
        base.Shoot(usedByPlayer);
        Bullet bullet = Instantiate(bulletprefab, transform.position, transform.rotation);
        bullet.Init(damage, bulletVelocity, usedByPlayer);
    }
}
