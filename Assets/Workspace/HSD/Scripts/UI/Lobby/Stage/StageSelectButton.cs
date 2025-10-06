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
    [SerializeField] Image _clearImage;
    private int _region;
    private int _stage;

    public void Init(UnityAction action, int region, int stage)
    {
        _region = region;
        _stage = stage;

        _selectButton.onClick.AddListener(action);
        _stageText.text = $"{_region} - {_stage}";
    }

    public void CheckStageOpen(bool stageOpen)
    {
        if (stageOpen)
        {
            _selectButton.interactable = true;
            _clearImage.color = Color.white;
        }
        else
        {
            _selectButton.interactable = false;
            _clearImage.color = Color.gray;
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
