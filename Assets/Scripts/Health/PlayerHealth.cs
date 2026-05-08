using System;
using UnityEngine;

public class PlayerHealth : Health
{
    public Action<bool> OnPlayerDeath; // is suicide ? 

    protected override void Die(bool swappedGun = false, bool melee = false)
    {
        base.Die();
        OnPlayerDeath?.Invoke(false);
    }

    public void Suicide()
    {
        base.Die();
        OnPlayerDeath?.Invoke(true);
        ScoreManager.Instance.ShowSuicideMessage();
    }
}
