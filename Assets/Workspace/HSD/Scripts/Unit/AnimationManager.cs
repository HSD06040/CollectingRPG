using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AnimationManager
{
    private UnitAnimatorData _animatorData;
    public Dictionary<AnimatorData, RuntimeAnimatorController> AnimatorDic;

    public async UniTask Init()
    {
        await AnimatorSetting();
    }

    private async UniTask AnimatorSetting()
    {
        _animatorData = await Addressables.LoadAssetAsync<UnitAnimatorData>("Data/UnitAnimatorData");

        AnimatorDic = new Dictionary<AnimatorData, RuntimeAnimatorController>(_animatorData.Animators.Length);

        _animatorData.SettingAnimationClip();

        foreach (var data in _animatorData.Animators)
        {
            CreateAnimator(data);
        }
    }

    private void CreateAnimator(AnimatorData data)
    {
        var newAnimator = new AnimatorOverrideController(_animatorData.BaseController);

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        newAnimator.GetOverrides(overrides);

        for (int i = 0; i < overrides.Count; i++)
        {
            var originalClip = overrides[i].Key;
            AnimationClip newClip = null;

            if (originalClip.name == "Melee_Attack")
                newClip = _animatorData.GetAnimationClip(data.AttackAnimationType);
            else if (originalClip.name == "Melee_Skill")
                newClip = _animatorData.GetAnimationClip(data.SkillAnimationType);

            if (newClip != null)
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(originalClip, newClip);
        }

        newAnimator.ApplyOverrides(overrides);

        if (!AnimatorDic.ContainsKey(data))
            AnimatorDic.Add(data, newAnimator);
    }

    public RuntimeAnimatorController GetAnimator(AnimatorData data)
    {
        if (!AnimatorDic.TryGetValue(data, out var animator))
        {
            CreateAnimator(data);
        }

        return AnimatorDic[data];
    }
}
