using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private UpgradeCollectedCharacterData _collectedCharacterData;
    [SerializeField] private UpgradePopupUI _popUpUI;
    public UpgradePopupUI PopUpUI => _popUpUI;

    [Header("UI")]
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private GameObject _magicStoneUI;
    [SerializeField] private TMP_Text _characterCountText;

    [Header("Button")]
    [SerializeField] private Button _characterButton;
    [SerializeField] private Button _magicStoneButton;

    private void Awake()
    {
        _characterButton.onClick.AddListener(() => SetActivePanel("CharacterUpgrade"));
        _magicStoneButton.onClick.AddListener(() => SetActivePanel("MagicStoneUpgrade"));
    }

    private void OnEnable()
    {
        ShowCharacterCountInfo();
        _popUpUI.gameObject.SetActive(false);        
    }

    public void ShowPopUp()
    {
        _popUpUI.gameObject.SetActive(true);
    }

    private void SetActivePanel(string activePanel)
    {
        _characterUI.SetActive(activePanel.Equals(_characterUI.name));
        _magicStoneUI.SetActive(activePanel.Equals(_magicStoneUI.name));
    }


    #region UI Output

    private void ShowCharacterCountInfo()
    {
        _characterCountText.text = $"보유 영웅 {_collectedCharacterData.CollectedCharacterCount}/{_collectedCharacterData.CharacterCount}";
        Debug.Log($"캐릭터 수 : {_collectedCharacterData.CollectedCharacterCount}");
    }

    #endregion

    
}