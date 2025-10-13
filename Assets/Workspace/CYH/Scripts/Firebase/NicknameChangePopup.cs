using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NicknameChangePopup : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private Button _confirmButton;

    private List<string> _badWords;


    private void Start()
    {
        _confirmButton.onClick.AddListener(OnClickChangeNickname);

        var topPanel = FindObjectOfType<TopPanelSystem>();
        if (topPanel != null)
        {
            _badWords = topPanel.BadWordList;
        }
    }

    /// <summary>
    /// 닉네임 변경 버튼 클릭 시 호출되는 메서드 (닉네임 유효성 검증)
    /// </summary>
    private void OnClickChangeNickname()
    {
        string newName = _nicknameInput.text.Trim();

        if (CheckGuestAccount()) return;

        // 미입력
        if (string.IsNullOrEmpty(newName))
        {
            PopupManager.Instance.ShowPopup("닉네임을 입력해주세요.");
            return;
        }

        // 비속어 포함
        if (!IsValid(newName))
        {
            PopupManager.Instance.ShowPopup("사용 불가능한 닉네임입니다.");
            _nicknameInput.text = string.Empty;
            return;
        }

        // 정상입력
        PopupManager.Instance.ShowPopup("닉네임이 변경되었습니다.");
        _nicknameInput.text = string.Empty;
        Manager.DB.SaveNicknameAsync(newName);
    }

    /// <summary>
    /// 게스트 계정 닉네임 변경 방지하는 메서드
    /// </summary>
    private bool CheckGuestAccount()
    {
        FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;

        if (currentUser != null && currentUser.IsAnonymous)
        {
            PopupManager.Instance.ShowConfirmationPopup("게스트 계정은 닉네임 변경이 불가합니다.",
                () => gameObject.SetActive(false), null);
            _nicknameInput.text = string.Empty;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 닉네임에 비속어가 포함되어 있는지 검사하는 메서드
    /// </summary>
    private bool IsValid(string nickname)
    {
        foreach (string word in _badWords)
        {
            if (nickname.Contains(word, System.StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }
}