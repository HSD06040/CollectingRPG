using UnityEngine;
using TMPro;

public class PlayerDataView : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _diamondText;

    public void UpdateUI(PlayerData data)
    {
        _playerNameText.text = data.PlayerName;
        _goldText.text = $"{data.Gold}";
        _diamondText.text = $"{data.Diamond}";
    }
}
