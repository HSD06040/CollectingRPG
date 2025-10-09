using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AugmentSlot : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] Image _icon;
    [SerializeField] Image _gradeSlot;
    [SerializeField] TMP_Text _description;

    public void Setup(AUGData data)
    {
        _icon.sprite = data.Icon;
        _description.text = data.Description;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => AugmentManager.Instance.SelectAugment(data));
    }
}
