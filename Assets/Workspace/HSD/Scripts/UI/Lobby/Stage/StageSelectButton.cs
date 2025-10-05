using Cysharp.Threading.Tasks.Triggers;
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

    public void CheckStageOpen(bool stageOpen)
    {
        if (stageOpen)
        {
            _selectButton.interactable = true;
            _selectButton.targetGraphic.color = Color.white;
        }
        else
        {
            _selectButton.interactable = false;
            _selectButton.targetGraphic.color = Color.gray;
        }
    }
}
