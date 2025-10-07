using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSynergyTextUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] CharacterUpgradeUnit _character;

    [Header("UI")]
    [SerializeField] TMP_Text _synergyText;
    [SerializeField] Image _synergyImage;
    private void Start()
    {
        if (_character != null && _character.Status != null)
        {
            _synergyText.text = _character.Status.Data.Synergy.ToString();
            if (Manager.Data.SynergyDB != null)
            {
                _synergyImage.sprite = Manager.Data.SynergyDB.GetSynergy((int)_character.Status.Data.Synergy).ActiveIcon;
            }
        }
    }
}