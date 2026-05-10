using UnityEngine;

public class DefaultWeapon : Melee
{
    private int animIndex;
    private CharacterAnim[] anims = { CharacterAnim.Punch1, CharacterAnim.Punch2 };

    public override void Animate()
    {
        wielder.GetComponent<CharacterAnimationController>().SetAnimation(anims[animIndex]);
        animIndex = (animIndex + 1) % anims.Length;
    }

    public void OnSwapDefault()
    {
        wielder.GetComponent<CharacterAnimationController>().SetAnimation(characterPose);
    }
}
