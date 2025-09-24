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
    [UIBind("PauseIcon")] Image _pauseImage;

    [SerializeField] Sprite[] _accelerateAmountSprites;

    [SerializeField] Sprite _pauseSprite;
    [SerializeField] Sprite _replaySprite;
    [SerializeField] string _pauseSpriteAddress;
    [SerializeField] string _replaySpriteAddress;

    private void OnEnable()
    {
        _accelerateButton.onClick.AddListener(NextAccelerate);
        Manager.Game.CurrentAccelerate.AddEvent(AccelerateAmountUpdate);

        _pauseButton.onClick.AddListener(ChangePause);
        Manager.Game.IsPause.AddEvent(PauseUpdate);
    }

    private void OnDisable()
    {
        _accelerateButton.onClick.RemoveListener(NextAccelerate);
        Manager.Game.CurrentAccelerate.RemoveEvent(AccelerateAmountUpdate);

        _pauseButton.onClick.RemoveListener(ChangePause);
        Manager.Game.IsPause.RemoveEvent(PauseUpdate);
    }

    private void NextAccelerate()
    {
        Manager.Game.NextAccelerate();
    }

    private void ChangePause()
    {
        Manager.Game.ChangePause();
    }

    private void AccelerateAmountUpdate(float amount)
    {
        _accelerateAmount.text = amount.ToString();
    }

    private void PauseUpdate(bool isPause)
    {        
        _pauseImage.sprite = isPause ? 
            Manager.Resources.SpriteGet(_pauseSpriteAddress) :
            Manager.Resources.SpriteGet(_replaySpriteAddress);
    }
}
