using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentPanel : MonoBehaviour
{
    [SerializeField] AugmentSlot[] _augmentSlots;
    [SerializeField] RectTransform _augmentPanel;
    [SerializeField] CanvasGroup _group;
    [SerializeField] CanvasGroup _panelGroup;
    [SerializeField] float _yOffset = 200f;
    [SerializeField] float _fadeDuration = .5f;
    [SerializeField] float _panelFadeDuration = .6f;
    private float _originY;
    private float _downY;

#if UNITY_EDITOR
    [Button]
    private void TestShow()
    {
        Show();
    }
#endif

    private void Awake()
    {
        _originY = _augmentPanel.anchoredPosition.y;
        _downY = _augmentPanel.anchoredPosition.y - _yOffset;

        foreach (var slot in _augmentSlots)
        {
            slot.OnSelected += Close;
        }
    }

    public void Show()
    {
        _group.blocksRaycasts = true;
        gameObject.SetActive(true);
        _augmentPanel.DOAnchorPosY(_downY, 0.01f);

        _group.FadeIn(_fadeDuration).Forget();
        _panelGroup.FadeIn(_panelFadeDuration).Forget();
        _augmentPanel.DOAnchorPosY(_originY, _panelFadeDuration);

        Setup();
    }

    private void Close()
    {
        _group.interactable = false;
        CloseAsync().Forget();
    }

    private async UniTask CloseAsync()
    {
        await _group.FadeOut(_fadeDuration);
        gameObject.SetActive(false);
    }

    public void Setup()
    {
        List<AUGData> augDatas = new List<AUGData>();

        for (int i = 0; i < 3; i++)
        {
            AUGData augmentData;

            do
            {
                augmentData = Manager.Data.AugmentChanceData.GetAugmentData();
            }
            while (augDatas.Contains(augmentData));

            augDatas.Add(augmentData);
        }

        for (int i = 0; i < _augmentSlots.Length; i++)
        {
            _augmentSlots[i].Setup(augDatas[i]);
        }
    }

}
