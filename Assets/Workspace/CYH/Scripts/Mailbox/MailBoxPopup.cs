using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailBoxPopup : MonoBehaviour
{
    [SerializeField] private PlayerMailBoxController _controller;

    [Header("List")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _mailItemPrefab;

    [Header("Button")]
    [SerializeField] private Button _receiveAllButton;
    [SerializeField] private Button _closePanelButton;

    [Header("Panel")]
    [SerializeField] private GameObject _receiveAllInfoPanel;
    [SerializeField] private GameObject _gold;
    [SerializeField] private GameObject _diamond;

    private bool _isDataBind = false;
    private bool _isReceivingAll = false;

    private Action onPanelActive;

    private void Apply(List<MailData> mails) => Init(mails);

    private void Start()
    {
        _receiveAllButton.onClick.RemoveAllListeners();
        _receiveAllButton.onClick.AddListener(OnClickReceiveAll);
        _closePanelButton.onClick.AddListener(() => _receiveAllInfoPanel.SetActive(false));
    }

    public void Init(List<MailData> mails)
    {
        if (mails == null)
        {
            Debug.Log("MailBoxPopup: mails == null");
            // 빈 우편함 아이콘 추가
            // 모두받기 버튼 interactable false
            return;
        }

        _receiveAllButton.interactable = true;

        // 기존 메일 제거
        for (int i = _content.childCount - 1; i >= 0; i--)
        {
            Destroy(_content.GetChild(i).gameObject);
        }

        // 메일 생성 + 바인딩
        foreach (var mail in mails)
        {
            GameObject mailObject = Instantiate(_mailItemPrefab, _content);

            // 비활성화로 생성 / 데이터 바인드 이후 OnEnable 실행
            mailObject.SetActive(false);

            MailItem mailItem = mailObject.GetComponent<MailItem>();
            if (mailItem != null)
                mailItem.Bind(mail, _controller);

            mailObject.SetActive(true);
        }
    }

    /// <summary>
    /// DB 데이터를 팝업에 실시간 연동 활성화/비활성화 하는 메서드
    /// </summary>
    /// <param name="enable">
    /// true: 실시간 연동 구독 시작
    /// false: 실시간 연동 구독 해제
    /// </param>
    public void EnableDataBind(bool enable)
    {
        if (_isDataBind == enable) return;
        _isDataBind = enable;

        if (_isDataBind)
            _controller.OnMailboxUpdated += Apply;
        else
            _controller.OnMailboxUpdated -= Apply;
    }

    private void OnDisable()
    {
        if (_isDataBind)
        {
            _controller.OnMailboxUpdated -= Apply;
            _isDataBind = false;
        }
    }

    private async void OnClickReceiveAll()
    {
        if (_isReceivingAll)
            return;

        _isReceivingAll = true;
        _receiveAllButton.interactable = false;

        var (totalGold, totalDiamond) = await _controller.ReceiveAllAsync();

        if (totalGold > 0)
        {
            if (!_receiveAllInfoPanel.activeSelf)
            {
                _receiveAllInfoPanel.SetActive(true);

            }

            _gold.SetActive(true);
            _receiveAllInfoPanel.GetComponent<ReceiveAllPanel>().SetGoldInfo(totalGold);

        }

        if (totalDiamond > 0)
        {
            if (!_receiveAllInfoPanel.activeSelf)
            {
                _receiveAllInfoPanel.SetActive(true);

            }

            _diamond.SetActive(true);
            _receiveAllInfoPanel.GetComponent<ReceiveAllPanel>().SetDiamondInfo(totalDiamond);
        }


        _isReceivingAll = false;
        //_receiveAllButton.interactable = true;
    }
}
