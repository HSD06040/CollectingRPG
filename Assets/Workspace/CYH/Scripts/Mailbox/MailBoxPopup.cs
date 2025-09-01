using System.Collections.Generic;
using UnityEngine;

public class MailBoxPopup : MonoBehaviour
{
    [SerializeField] private PlayerMailBoxController _controller;

    [Header("List")]
    [SerializeField] private RectTransform _content;       
    [SerializeField] private GameObject _mailItemPrefab;  

    private bool _isDataBind = false;

    private void Apply(List<MailData> mails) => Init(mails);

    public void Init(List<MailData> mails)
    {
        if (mails == null)
        {
            Debug.Log("MailBoxPopup: mails == null");
            return;
        }
        
        Debug.Log("[MailBoxPopup] Init(리스트 UI를 현재 메일 목록으로 갱신)");
        // 기존 메일 제거
        for (int i = _content.childCount - 1; i >= 0; i--)
        {
            Destroy(_content.GetChild(i).gameObject);
        }

        // 메일 생성 + 바인딩
        foreach (var mail in mails)
        {
            GameObject mailItem = Instantiate(_mailItemPrefab, _content);
            MailItem mailItemView = mailItem.GetComponent<MailItem>();
            if (mailItemView != null)
                mailItemView.Bind(mail, _controller);
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
}
