using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInPanel : MonoBehaviour
{
    [SerializeField] private Button _signOutButton;

    private void Start()
    {
        _signOutButton.onClick.AddListener(() => 
        { 
            FirebaseManager.Auth.SignOut();
            Debug.Log("로그아웃");
        });
    }
}
