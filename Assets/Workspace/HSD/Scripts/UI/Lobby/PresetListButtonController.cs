using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetListButtonController : MonoBehaviour
{
    private const int BUTTON_MOVE_ID = 1134256432;
    private float _currentY = 0;

    [Header("Reference")]
    [SerializeField] Image[] _presetButtonList;
    [SerializeField] Sprite[] _presetButtonImages;

    [SerializeField] Button _openButton;
    [SerializeField] TMP_Text _currentIdxText;
    [SerializeField] GameObject _presetButtons;
    [SerializeField] Transform _targetTransform;
    private bool _isOpen;

    private void Start()
    {
        _currentY = _presetButtons.transform.position.y;
    }

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Switch);
        UpdateButtons();
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Switch);
    }

    public void SetCurrentIdx(int currentIdx)
    {
        _currentIdxText.text = currentIdx.ToString();
    }

    private void Switch()
    {
        if (_isOpen)
        {
            DeActive();
        }
        else
        {
            Active();
            UpdateButtons();
        }        
    }

    private void Active()
    {
        DOTween.Kill(BUTTON_MOVE_ID);

        _presetButtons.transform.DOMoveY(_targetTransform.position.y, .3f).SetId(BUTTON_MOVE_ID);
        _isOpen = true;
    }

    private void DeActive()
    {
        DOTween.Kill(BUTTON_MOVE_ID);

        _presetButtons.transform.DOMoveY(_currentY, .3f).SetId(BUTTON_MOVE_ID);
        _isOpen = false;
    }

    private void UpdateButtons()
    {
        int index = Manager.Data.PresetDB.SelectedPresetIndex;

        for(int i = 0; i < _presetButtonList.Length; i++)
        {
            if (index == i) _presetButtonList[i].sprite = _presetButtonImages[0];
            else _presetButtonList[i].sprite = _presetButtonImages[1];
        }
    }
}
