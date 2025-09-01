using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopupManager : MonoBehaviour
{
    [SerializeField] private PlayerDataController _playerDataController;
    [SerializeField] private PlayerMailBoxController _playerMailBoxController;
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

    [Header("MailBox")]
    [SerializeField] private GameObject _mailboxPopup;
    [SerializeField] private MailBoxPopup _mailPopup;
    [SerializeField] private Button _mailboxButton;


    private void Start()
    {
        _playerProfileButton.onClick.AddListener(() =>
        {
            ShowPopup_playerProfile();
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

        _mailboxButton.onClick.AddListener(() =>
        {
            ShowPopup_mail();
        });
    }

    public void ShowPopup_playerProfile()
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

    public void ShowPopup_mail()
    {
        HideAllPopup();

        if (_mailboxPopup != null)
        {
            _mailPopup.Init(_playerMailBoxController.Mail);
            _mailPopup.EnableDataBind(true);
            _mailboxPopup.SetActive(true);
        }

        gameObject.SetActive(true);
    }

    private void HideAllPopup()
    {
        if (_playerProfilePopup != null) _playerProfilePopup.SetActive(false);
        if (_googleLinkPopup != null) _googleLinkPopup.SetActive(false);
        if (_mailboxPopup != null) _mailboxPopup.SetActive(false);
    }
}
