using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StageSelectButton : MonoBehaviour
{
    [SerializeField] Button _selectButton;
    [SerializeField] TMP_Text _stageText;

    public void Init(UnityAction action, int region, int stage)
    {
        _selectButton.onClick.AddListener(action);
        _stageText.text = $"{region} - {stage}";
    }
}
