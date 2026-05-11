using System;
using UnityEngine;

public class PlayerHealth : Health
{
    public Action<bool> OnPlayerDeath; // is suicide ? 

    protected override void Die(bool swappedGun = false, bool melee = false)
    {
        base.Die();
        OnPlayerDeath?.Invoke(false);
        Explode();
        gameObject.SetActive(false);
    }

    public void Suicide()
    {
        base.Die();
        OnPlayerDeath?.Invoke(true);
        ScoreManager.Instance.ShowSuicideMessage();
    }

    private void Explode()
    {
        Gibs g = GetComponentInChildren<Gibs>();
        if (g != null)
        {
            g.Explode();
        }
    }
}
