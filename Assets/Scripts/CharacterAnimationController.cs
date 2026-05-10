using System;
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

    [Serializable]
    public class AnimationTrackRecord
    {
        public CharacterAnim animName;
        [SpineAnimation] public string anim;
        public CharacterAnimTrack track;
        public bool loopingAnimation;
    }

    [SerializeField] private AnimationTrackRecord[] trackRecords;

    private Dictionary<CharacterAnim, (string, int, bool)> animToTrackMap = new Dictionary<CharacterAnim, (string, int, bool)>();

    private void Start()
    {
        foreach (AnimationTrackRecord record in trackRecords)
        {
            animToTrackMap.Add(record.animName, (record.anim, (int)record.track, record.loopingAnimation));
        }
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
    Walk, PoseHands, PosePistol, PoseRifle, Punch1, Punch2, ShootPistol, ShootRifle
}

//  Passive is things like walking/running/idle, something you do without thinking
//  Pose is anything that dictates a pose like holding your arms out or positioning your arms as if you carry a gun
//  Action is anything that dictates a direct action like punching or shooting
public enum CharacterAnimTrack
{
    Passive, Pose, Action
}
