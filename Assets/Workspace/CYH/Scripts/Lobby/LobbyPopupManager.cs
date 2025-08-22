using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopupManager : MonoBehaviour
{
    [SerializeField] private PlayerDataController _playerDataController;
    [SerializeField] private GoogleAdMob _googleAdMob;
    
    [Header("PlayerProfile")]
    [SerializeField] private GameObject _playerProfilePopup;
    [SerializeField] private PlayerProfilePopup _profilePopup;
    [SerializeField] private Button _playerProfileButton;

    [Header("Ad")]
    [SerializeField] private Button _adButton;


    private void Start()
    {
        _playerProfileButton.onClick.AddListener(() =>
        {
            Debug.Log("프로필 버튼 클릭");
            ShowPopup();
        });

        _adButton.onClick.AddListener(() =>
        {
            Manager.Popup.ShowConfirmationPopup(
                "Watch Ad",
                () => _googleAdMob.ShowAd(),
                () => gameObject.SetActive(false));
        });
    }

    public void ShowPopup()
    {
        HideAllPopup();

        if (_playerProfilePopup != null)
        {
            _profilePopup.Init(_playerDataController.Data);
            _profilePopup.EnableDataBind(true);          
            _playerProfilePopup.SetActive(true);
        }

        gameObject.SetActive(true);
    }

    private void HideAllPopup()
    {
        if (_playerProfilePopup != null)
        {
            _playerProfilePopup.SetActive(false);
        }
    }
}
