using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class LogoutButton : MonoBehaviour
{
    [SerializeField] private Button _logoutButton;

    private void Reset()
    {
        if (_logoutButton == null)
            _logoutButton = GetComponent<Button>();
    }

    private void Awake()
    {
        if (_logoutButton == null)
        {
            Debug.LogError("[Auth] 로그아웃 버튼 미할당");
            return;
        }

        _logoutButton.onClick.AddListener(OnClick_Logout);
    }

    private void OnDestroy()
    {
        if (_logoutButton != null)
            _logoutButton.onClick.RemoveListener(OnClick_Logout);
    }

    private void OnClick_Logout()
    {
        var auth = FirebaseManager.Auth;
        if (auth == null)
        {
            Debug.LogError("[Auth] FirebaseAuth 인스턴스가 없습니다.");
            return;
        }

        if (auth.CurrentUser == null)
        {
            Debug.Log("[Auth] 현재 로그인된 사용자가 없습니다.");
            return;
        }

        string uid = auth.CurrentUser.UserId;
        string name = auth.CurrentUser.DisplayName;

        // 로그아웃
        auth.SignOut();

        Debug.Log($"[Auth] 로그아웃 완료 - 이전 UID={uid}, 닉네임={name}");
    }
}