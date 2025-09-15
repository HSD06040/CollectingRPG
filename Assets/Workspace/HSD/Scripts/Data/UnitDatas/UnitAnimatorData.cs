using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimatorData", menuName = "UnitAnimatorData")]
public class UnitAnimatorData : ScriptableObject
{
    [Serializable]
    public struct AnimatorData
    {
        public string AnimatorName;
        public AnimationClip AttackAnimationClip;
        public AnimationClip SkillAnimationClip;
    }

    public RuntimeAnimatorController BaseController;
    public AnimatorData[] Animators;
}
