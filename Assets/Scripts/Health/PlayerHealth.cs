using UnityEngine;

public class PlayerHealth : Health
{
    protected override void Die(bool swappedGun = false, bool melee = false)
    {
        base.Die();
    }
}
