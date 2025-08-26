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

    [Header("AccountLink")]
    [SerializeField] private GameObject _googleLinkPopup;
    [SerializeField] private Button _accountLinkButton;


    private void Start()
    {
        _playerProfileButton.onClick.AddListener(() =>
        {
            ShowPopup();
        });

        _adButton.onClick.AddListener(() =>
        {
            Manager.Popup.ShowConfirmationPopup(
                "Watch Ad",
                () => _googleAdMob.ShowAd(),
                () => gameObject.SetActive(false));
        });

        _accountLinkButton.onClick.AddListener(() =>
        {
            _googleLinkPopup.SetActive(true);
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
