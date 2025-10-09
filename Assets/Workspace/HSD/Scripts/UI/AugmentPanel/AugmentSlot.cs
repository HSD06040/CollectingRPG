using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AugmentSlot : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] CanvasGroup _group;
    [SerializeField] Image _icon;
    [SerializeField] Image _gradeSlot;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _gradeText;
    [SerializeField] TMP_Text _description;
    private AUGData _data;

    [Header("Animation")]
    [SerializeField] float _yOffset;
    [SerializeField] float _duration;
    private RectTransform _transform;
    private float _originY;
    private float _upY;

    public event Action OnSelected;

#if UNITY_EDITOR
    [Button]
    private void TestOffsetSetting()
    {
        OffsetSetting();
    }
#endif

    private void Awake()
    {
        _transform = (RectTransform)transform;
        OffsetSetting();
    }

    private void OffsetSetting()
    {
        _originY = _transform.anchoredPosition.y;
        _upY = _transform.anchoredPosition.y + _yOffset;
    }

    public void Setup(AUGData data)
    {
        _group.alpha = 1f;
        _group.interactable = true;
        _group.blocksRaycasts = true;

        _transform.DOAnchorPosY(_originY, .01f);

        _data = data;

        _icon.sprite = data.Icon;
        _nameText.text = data.Name;
        _description.text = data.Description;
        _gradeText.text = data.Grade.ToString();

        _button.interactable = true;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(ButtonClickEvent);
    }

    private void ButtonClickEvent()
    {
        OnSelected?.Invoke();
        SelectAnimation();
        AugmentManager.Instance.SelectAugment(_data);
    }

    private void SelectAnimation()
    {
        _transform.DOAnchorPosY(_upY, _duration).SetUpdate(true);
        _group.FadeOut(_duration).Forget();
    }
}
