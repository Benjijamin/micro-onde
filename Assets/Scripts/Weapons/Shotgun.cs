using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private int pelletCount;
    [SerializeField] private float coneAngle;

    protected override void Shoot(bool usedByPlayer)
    {
        base.Shoot(usedByPlayer);
        for (int i = 0; i < pelletCount; i++)
        {
            Bullet bullet = Instantiate(bulletprefab, muzzleTransform.position, Quaternion.Euler(0, 0, Random.Range(-coneAngle / 2, coneAngle / 2)) * transform.rotation);
            bullet.Init(damage, bulletVelocity, usedByPlayer);
        }
    }
}
