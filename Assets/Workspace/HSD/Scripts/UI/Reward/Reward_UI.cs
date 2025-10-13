using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Reward_UI : MonoBehaviour
{
    [SerializeField] Button _closeButton;
    [SerializeField] CanvasGroup _rewardSlotGroup;
    [SerializeField] CanvasGroup _rewardGroup;
    [SerializeField] Transform _content;
    [SerializeField] GameObject _rewardPanel;
    [SerializeField] Reward_Slot_UI[] _rewardSlots;
    [SerializeField] float _fadeDuration = 0.5f;

    [SerializeField] Ease _ease = Ease.InCubic;
    [SerializeField] float _animationDuration = 0.5f;
    private Vector2 _origin;
    private Vector2 _down;

#if UNITY_EDITOR
    [Button]
    private void TestShow()
    {
        PlayShowAnimation().Forget();
    }
    [Button]
    private void TestHide()
    {
        PlayHideAnimation().Forget();
    }
#endif
    private void Awake()
    {
        _origin = ((RectTransform)_rewardPanel.transform).anchoredPosition;
        _down = new Vector2(_origin.x, _origin.y - 400f);

        ((RectTransform)_rewardPanel.transform).anchoredPosition = _down;

        _rewardGroup.alpha = 0f;
        _rewardGroup.blocksRaycasts = false;
        _rewardGroup.interactable = false;
        _rewardSlotGroup.alpha = 0f;
        _rewardSlotGroup.blocksRaycasts = false;
        _rewardGroup.interactable = false;
    }

    public void Show(OutGameRewardData[] outGameRewardDatas)
    {        
        int count = 0;
        for (int i = 0; i < outGameRewardDatas.Length; i++)
        {
            count++;
            var rewardData = outGameRewardDatas[i];
            var icon = rewardData.GetRewardSprite();

            _rewardSlots[i].Setup(icon, rewardData.Amount);
        }

        for (int i = count; i < _rewardSlots.Length; i++)
        {
            _rewardSlots[i].gameObject.SetActive(false);
        }

        PlayShowAnimation().Forget();
    }

    public void Show(StageInGameRewardType[] inGameRewardDatas, UnityAction action = null)
    {
        ButtonEventSubcribe(action);

        int count = 0;
        for (int i = 0; i < inGameRewardDatas.Length; i++)
        {
            count++;
            var rewardData = inGameRewardDatas[i];
            var icon = rewardData.GetRewardSprite();

            _rewardSlots[i].Setup(icon, rewardData.Amount);
        }

        for (int i = count; i < _rewardSlots.Length; i++)
        {
            _rewardSlots[i].gameObject.SetActive(false);
        }

        PlayShowAnimation().Forget();
    }

    public void Close()
    {
        _closeButton.interactable = false;
        PlayHideAnimation().Forget();
    }

    private async UniTask PlayShowAnimation()
    {
        _rewardPanel.transform.localPosition = _down;
        _rewardGroup.FadeIn(_fadeDuration).Forget();
        ((RectTransform)_rewardPanel.transform).DOAnchorPos(_origin, _animationDuration).SetEase(_ease).SetUpdate(true);
        await _rewardSlotGroup.FadeIn(_animationDuration);
    }

    private async UniTask PlayHideAnimation()
    {
        _rewardSlotGroup.FadeOut(_fadeDuration, false).Forget();
        await _rewardGroup.FadeOut(_fadeDuration, false);
    }

    private void ButtonEventSubcribe(UnityAction unityAction)
    {
        _closeButton.interactable = true;
        _closeButton.onClick.RemoveAllListeners();

        if (unityAction == null)
        {
            _closeButton.onClick.AddListener(Close);
            return;   
        }

        _closeButton.onClick.AddListener(unityAction);
        _closeButton.onClick.AddListener(Close);
    }
}
