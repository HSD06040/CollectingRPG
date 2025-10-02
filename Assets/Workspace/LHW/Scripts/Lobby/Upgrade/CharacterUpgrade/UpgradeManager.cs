using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private UpgradeCharacterPopupUI _characterPopUpUI;
    public UpgradeCharacterPopupUI CharPopUpUI => _characterPopUpUI;
    [SerializeField] private UpgradeStonePopupUI _stonePopUpUI;
    public UpgradeStonePopupUI StonePopUpUI => _stonePopUpUI;

    [Header("UI")]
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private GameObject _magicStoneUI;

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
        _characterPopUpUI.gameObject.SetActive(false);        
    }

    public void ShowCharacterPopUp()
    {
        _characterPopUpUI.gameObject.SetActive(true);
    }

    public void ShowStonePopUp()
    {
        _stonePopUpUI.gameObject.SetActive(true);
    }

    private void SetActivePanel(string activePanel)
    {
        _characterUI.SetActive(activePanel.Equals(_characterUI.name));
        _magicStoneUI.SetActive(activePanel.Equals(_magicStoneUI.name));
    }   
}