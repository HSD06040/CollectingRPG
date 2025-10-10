using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UI_Utils
{
    public static void Lerp(this Slider slider, float endValue, float duration = 0.5f, Ease ease = Ease.OutQuad)
    {
        DOTween.Kill(slider, id: "Value");

        slider
            .DOValue(endValue, duration)
            .SetEase(ease)
            .SetId("Value")
            .SetTarget(slider);
    }

    public static void LerpText(this TMP_Text text, int endValue, float duration = 0.5f, Ease ease = Ease.OutCubic)
    {
        int startValue = int.TryParse(text.text, out var parsed) ? parsed : 0;

        DOTween.Kill(text, id: "Value");

        DOTween.To(
            () => startValue,
            x => {
                startValue = x;
                text.text = Utils.ToAbbreviation(x);
            },
            endValue,
            duration
        )
        .SetEase(ease)
        .SetId("Value")
        .SetTarget(text);
    }

    public async static UniTask FadeIn(this CanvasGroup canvasGroup, float fadeDuration)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        await canvasGroup.DOFade(1f, fadeDuration).SetUpdate(true).SetEase(Ease.Linear).AsyncWaitForCompletion();

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public async static UniTask FadeOut(this CanvasGroup canvasGroup, float fadeDuration, bool beforeDeActive = true)
    {
        if(beforeDeActive)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }        

        await canvasGroup.DOFade(0f, fadeDuration).SetUpdate(true).SetEase(Ease.Linear).AsyncWaitForCompletion();

        if(!beforeDeActive)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }        
    }

    public static void Reset(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false; 
        canvasGroup.blocksRaycasts = false;        
    }

    public static Sprite GetRewardSprite(this OutGameRewardData rewardData)
    {
        return rewardData.RewardType switch
        {
            OutGameRewardType.Diamond => Manager.Resources.SpriteLoad("Diamond"),
            OutGameRewardType.Gold => Manager.Resources.SpriteLoad("Gold"),
            OutGameRewardType.Exp => Manager.Resources.SpriteLoad("Exp"),
            _ => null,
        };
    }

    public static Sprite GetRewardSprite(this StageInGameRewardType rewardData)
    {
        return rewardData.RewardType switch
        {
            InGameRewardType.Silver => Manager.Resources.SpriteLoad("Silver"),
            InGameRewardType.Energy => Manager.Resources.SpriteLoad("Energy"),
            InGameRewardType.MagicStone => rewardData.MagicStoneData != null ? rewardData.MagicStoneData.Icon : null,
            _ => null,
        };
    }
}
