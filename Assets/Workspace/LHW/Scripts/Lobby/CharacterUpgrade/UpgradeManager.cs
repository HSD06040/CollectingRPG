using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private UpgradeCollectedCharacterData _collectedCharacterData;
    [SerializeField] private UpgradePopupUI _popUpUI;
    public UpgradePopupUI PopUpUI => _popUpUI;

    [Header("UI")]
    [SerializeField] private TMP_Text _characterCountText;

    private void OnEnable()
    {
        ShowCharacterCountInfo();
        _popUpUI.gameObject.SetActive(false); 
    }

    public void ShowPopUp()
    {
        _popUpUI.gameObject.SetActive(true);
    }

    #region UI Output

    private void ShowCharacterCountInfo()
    {
        _characterCountText.text = $"보유 영웅 {_collectedCharacterData.CollectedCharacterCount}/{_collectedCharacterData.CharacterCount}";
        Debug.Log($"캐릭터 수 : {_collectedCharacterData.CollectedCharacterCount}");
    }

    #endregion
}