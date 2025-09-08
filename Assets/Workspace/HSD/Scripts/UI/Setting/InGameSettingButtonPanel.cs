using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameSettingButtonPanel : UIBase
{
    [UIBind("AccelerateButton")] Button _accelerateButton;
    [UIBind("AccelerateAmount")] TMP_Text _accelerateAmount;

    [UIBind("PauseButton")] Button _pauseButton;

    [SerializeField] Sprite[] _accelerateAmountSprites;

    private void OnEnable()
    {
        _accelerateButton.onClick.AddListener(NextAccelerate);
        Manager.Game.CurrentAccelerate.AddEvent(AccelerateAmountUpdate);
    }

    private void OnDisable()
    {
        _accelerateButton.onClick.RemoveListener(NextAccelerate);
        Manager.Game.CurrentAccelerate.RemoveEvent(AccelerateAmountUpdate);
    }

    private void NextAccelerate()
    {
        Manager.Game.NextAccelerate();
    }

    private void AccelerateAmountUpdate(float amount)
    {
        _accelerateAmount.text = amount.ToString();
    }
}
