using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Random = UnityEngine.Random;


public class GuestSignIn : MonoBehaviour
{
    [SerializeField] private Button _guestLoginButton;

    private bool _isClicked;
    public Action LoginCompleted { get; set; }

    private void Start()
    {
        _guestLoginButton.onClick.AddListener(() =>
        {
            if (!_isClicked)
            {
                OnClick_GuestLogin();
            }
        });
    }

    private void OnClick_GuestLogin()
    {
        _isClicked = true;
       
        // �Խ�Ʈ �α��� ���� ���� üũ
        if (FirebaseManager.Auth.CurrentUser != null)
        {
            Debug.LogError($"���� UID : {FirebaseManager.Auth.CurrentUser.UserId}  " +
                $"/ ���� �г��� : {FirebaseManager.Auth.CurrentUser.DisplayName}");
            _isClicked = false;
            return;
        }

        FirebaseManager.Auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(async task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("�Խ�Ʈ �α��� ���");
                _isClicked = false;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError($"�Խ�Ʈ �α��� ���� / ����: {task.Exception}");
                _isClicked = false;
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;

            FirebaseUser user = FirebaseManager.Auth.CurrentUser;

            Debug.Log("�Խ�Ʈ ���� �Ϸ�");

            await user.ReloadAsync();

            // �Խ�Ʈ �г��� ���� 
            await SetGuestNickname(user);
            await user.ReloadAsync();

            Debug.Log("------���� ����(GuestLogin)------");
            Debug.Log($"���� �г��� : {user.DisplayName}");
            Debug.Log($"���� ID : {user.UserId}");
            Debug.Log($"�̸��� : {user.Email}");

            // LoginPanel -> GameStartPanel �� ����
            if (user != null)
            {
                Debug.Log("�Խ�Ʈ ���� ������Ʈ �Ϸ�. GameStart�г� Ȱ��ȭ");
                LoginCompleted?.Invoke();
                _isClicked = false;
            }
        });
    }

    /// <summary>
    /// �͸������ DisplayName�� "�Խ�Ʈ + ��������"�� �����ϴ� �޼��� 
    /// ����: GuestLogin
    /// </summary>
    /// <param name="currentUser">�г����� ������ ����</param>
    public static async Task SetGuestNickname(FirebaseUser currentUser)
    {
        UserProfile profile = new UserProfile();
        profile.DisplayName = $"�Խ�Ʈ{Random.Range(1000, 10000)}";

        await currentUser.UpdateUserProfileAsync(profile);
        // �ʱ�ȭ
        await currentUser.ReloadAsync();
        
        // Firebase DB�� �г��� ����
        await SaveNicknameAsync();
        await currentUser.ReloadAsync();

        Debug.Log("�г��� ���� ����");
        Debug.Log($"����� ���� �г��� : {currentUser.DisplayName}");
    }

    public static async Task<bool> SaveNicknameAsync()
    {
        FirebaseUser currentUser = FirebaseManager.Auth.CurrentUser;
        string uid = currentUser.UserId;
        string userNickname = FirebaseManager.Auth.CurrentUser.DisplayName;

        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        // �͸���� RankData ���� x
        if (currentUser.IsAnonymous)
        {
            dictionary[$"UserData/{uid}/Nickname"] = userNickname;
        }

        else
        {
            dictionary[$"UserData/{uid}/Nickname"] = userNickname;
            dictionary[$"RankData/{uid}/Nickname"] = userNickname;
        }

        var task = FirebaseManager.DataReference.UpdateChildrenAsync(dictionary);
        await task;

        if (task.IsCompletedSuccessfully)
        {
            Debug.Log("UserData / RankData �� �г��� ���� ����");
            return true;
        }
        else
        {
            Debug.LogError("�г��� ���� ����");
            return false;
        }
    }
}
