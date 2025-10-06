using Cysharp.Threading.Tasks.Triggers;
using System;
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
    [SerializeField] Image _clearImage;
    [SerializeField] Image _outline;
    
    private int _region;
    private int _stage;
    private static Color _defaultOutLineColor = new Color(157f / 255f, 74f / 255f, 185f / 255f, 1f);
    private static Color _selectedOutLineColor = Color.white;
    private static Color _defualtButtonColor = new Color(127f / 255f, 105f / 255f, 197f / 255f, 170f / 255f);
    private static Color _buttonSelectColor = new Color(66f / 255f, 67f / 255f, 185f / 255f, 1f);

    public void Init(UnityAction action, int region, int stage)
    {
        _region = region;
        _stage = stage;

        _selectButton.onClick.AddListener(action);
        _selectButton.onClick.AddListener(Select);

        _stageText.text = $"{_region} - {_stage}";

        DeSelect();
    }
    
    public void Select()
    {
        _outline.color = _selectedOutLineColor;
        _selectButton.targetGraphic.color = _buttonSelectColor;
    }

    public void DeSelect()
    {
        _outline.color = _defaultOutLineColor;
        _selectButton.targetGraphic.color = _defualtButtonColor;
    }

    public void CheckStageOpen(bool stageOpen)
    {
        if (stageOpen)
        {
            _selectButton.interactable = true;
            _clearImage.color = Color.white;
            DeSelect();
        }
        else
        {
            _selectButton.interactable = false;
            _clearImage.color = Color.gray;
            _outline.color = Color.gray;
            _selectButton.targetGraphic.color = new Color(1f, 1f, 1f, 100f / 255f);
        }
    }

    public void CheckClear(StageData stageData)
    {
        if(stageData.HasClearProof(_stage))
        {
            _clearImage.sprite = Manager.Resources.SpriteLoad("OpenChest");
        }
        else
        {
            _clearImage.sprite = Manager.Resources.SpriteLoad("CloseChest");
        }
    }
}
