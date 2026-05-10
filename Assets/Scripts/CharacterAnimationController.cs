using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeleton;

    //  walk    = 0
    //  pose    = 1
    //  action  = 2

    [SpineAnimation][SerializeField] private string walkAnim;

    [SpineAnimation][SerializeField] private string poseHands;
    [SpineAnimation][SerializeField] private string posePistol;
    [SpineAnimation][SerializeField] private string poseRifle;

    [SpineAnimation][SerializeField] private string punch1;
    [SpineAnimation][SerializeField] private string punch2;

    private Dictionary<CharacterAnim, (string, int, bool)> animToTrackMap = new Dictionary<CharacterAnim, (string, int, bool)>();

    private void Start()
    {
        animToTrackMap.Add(CharacterAnim.Walk, (walkAnim, (int)CharacterAnimTrack.Passive, true));

        animToTrackMap.Add(CharacterAnim.PoseHands, (poseHands, (int)CharacterAnimTrack.Pose, true));
        animToTrackMap.Add(CharacterAnim.PosePistol, (posePistol, (int)CharacterAnimTrack.Pose, true));
        animToTrackMap.Add(CharacterAnim.PoseRifle, (poseRifle, (int)CharacterAnimTrack.Pose, true));

        animToTrackMap.Add(CharacterAnim.Punch1, (punch1, (int)CharacterAnimTrack.Action, false));
        animToTrackMap.Add(CharacterAnim.Punch2, (punch2, (int)CharacterAnimTrack.Action, false));
    }

    public void SetAnimation(CharacterAnim anim)
    {
        if(animToTrackMap[anim].Item2 == (int)CharacterAnimTrack.Pose) { CancelAnimation((int)CharacterAnimTrack.Action); }
        skeleton.AnimationState.SetAnimation(animToTrackMap[anim].Item2, animToTrackMap[anim].Item1, animToTrackMap[anim].Item3);
    }

    public void AddAnimation(CharacterAnim anim, float delay = 0f)
    {
        skeleton.AnimationState.AddAnimation(animToTrackMap[anim].Item2, animToTrackMap[anim].Item1, animToTrackMap[anim].Item3, delay);
    }

    public void CancelAnimation(CharacterAnim anim)
    {
        skeleton.AnimationState.SetEmptyAnimation(animToTrackMap[anim].Item2, 0.5f);
    }

    public void CancelAnimation(int trackIndex)
    {
        skeleton.AnimationState.SetEmptyAnimation(trackIndex, 0.5f);
    }

    public string GetAnimation(CharacterAnim anim)
    {
        return animToTrackMap[anim].Item1;
    }

    public TrackEntry GetCurrentAnimation(CharacterAnimTrack track)
    {
        return skeleton.AnimationState.GetCurrent((int)track);
    }

    public bool IsAnimPlaying(CharacterAnim anim)
    {
        if (skeleton.AnimationState.GetCurrent(animToTrackMap[anim].Item2) == null || skeleton.AnimationState.GetCurrent(animToTrackMap[anim].Item2).IsEmptyAnimation) { return false; }
        return skeleton.AnimationState.GetCurrent(animToTrackMap[anim].Item2).Animation.Name == animToTrackMap[anim].Item1;
    }
}

public enum CharacterAnim
{
    Walk, PoseHands, PosePistol, PoseRifle, Punch1, Punch2
}

//  Passive is things like walking/running/idle, something you do without thinking
//  Pose is anything that dictates a pose like holding your arms out or positioning your arms as if you carry a gun
//  Action is anything that dictates a direct action like punching or shooting
public enum CharacterAnimTrack
{
    Passive, Pose, Action
}
