using UnityEngine;

public class DefaultWeapon : Melee
{
    public override void Animate()
    {
        animator.Play("Punch", 0, 0);
    }
}
