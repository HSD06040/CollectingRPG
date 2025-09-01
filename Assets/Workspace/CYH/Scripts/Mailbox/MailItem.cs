using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailItem : MonoBehaviour
{
    [Header("Reward")]
    [SerializeField] private Image _rewardImage;
    [SerializeField] private Sprite _goldSprite;     
    [SerializeField] private Sprite _diamondSprite;  
    [SerializeField] private TMP_Text _rewardText;

    [Header("Info")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _bodyText;

    [Header("Button")]
    [SerializeField] private Button _receiveButton;
    [SerializeField] private TMP_Text _receiveText;
    [SerializeField] private TMP_Text _expireDateText;
    [SerializeField] private GameObject _timeBoxImage;
    [SerializeField] private GameObject _badgeImage;

    private MailData _data;
    private PlayerMailBoxController _controller;
    private bool _isClicked = true;

    public void Bind(MailData data, PlayerMailBoxController controller)
    {
        _data = data;
        _controller = controller;
        _isClicked = false;

        _titleText.text = data.Title;
        _bodyText.text = data.Body;
        _rewardText.text = (data.Gold == 0) ? $"{data.Diamond}" : $"{data.Gold}";
        _rewardImage.sprite = (data.Gold == 0) ? _diamondSprite : _goldSprite;

        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        if (data.ExpireDate > currentTime)
        {
            //ExpireDate까지 남은 시간을 TimeSpan으로 변환
            TimeSpan remain = TimeSpan.FromMilliseconds(data.ExpireDate - currentTime);

            if (remain.TotalDays >= 1)
                _expireDateText.text = $"{(int)remain.TotalDays}일 남음";
            else if (remain.TotalHours >= 1)
                _expireDateText.text = $"{(int)remain.TotalHours}시간 남음";
            else if (remain.TotalMinutes >= 1)
                _expireDateText.text = $"{(int)remain.TotalMinutes}분 남음";
            else
                _expireDateText.text = $"{(int)remain.TotalSeconds}초 남음";
        }
        else
        {
            // 기간 만료 UI 변경
            RectTransform receiveTextPos = _receiveText.GetComponent<RectTransform>();
            Vector2 anchoredPos = receiveTextPos.anchoredPosition;
            anchoredPos.y = -50f;
            receiveTextPos.anchoredPosition = anchoredPos;
            _receiveText.text = "기간 만료";
            _badgeImage.SetActive(false);
            _timeBoxImage.SetActive(false);
        }

        // 버튼 활성화 여부 -> 수령 전 & 만료x
        bool expired = data.IsExpired(currentTime);
        _receiveButton.interactable = !data.IsReceived && !expired;

        _receiveButton.onClick.RemoveAllListeners();
        _receiveButton.onClick.AddListener(() =>
        {
            if (!_isClicked && !data.IsReceived)
            {
                Debug.Log("_receiveButton");
                _isClicked = true;
                _controller.ReceiveReward(_data.MailId);

                //RectTransform receiveTextPos = _receiveText.GetComponent<RectTransform>();
                //Vector2 anchoredPos = receiveTextPos.anchoredPosition;
                //anchoredPos.y = -50f;
                //receiveTextPos.anchoredPosition = anchoredPos;
                //_receiveText.text = "수령 완료";
                //_badgeImage.SetActive(false);
                //_timeBoxImage.SetActive(false);

            }
        });
    }
}
