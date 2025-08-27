using UnityEngine;
using Firebase.Database;
using System;

public class PlayerDataController : MonoBehaviour
{
    [SerializeField] private PlayerDataView _view;
 
    private DatabaseReference _userRef;
    private PlayerData _data;

    public PlayerData Data { get { return _data; } }

    public Action<PlayerData> OnPlayerProfilePopupUpdated;
    public Action<PlayerData> OnUpdateUI;


    private void Start()
    {
        InitAsync();
        OnUpdateUI += _view.UpdateUI;
    }

    private void OnEnable()
    {
        StartListeningToPlayerData();
    }

    private void OnDisable()
    {
        StopListeningToPlayerData();
    }

    public async void InitAsync()
    {
        _data = await DBManager.Instance.LoadLobbyDataAsync();
        _view.UpdateUI(_data);
        RefreshUI(_data);
    }

    public void RefreshUI(PlayerData data)
    {
        _data = data;
        OnUpdateUI?.Invoke(data);
        OnPlayerProfilePopupUpdated?.Invoke(data);
    }

    private void StartListeningToPlayerData()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        _userRef = FirebaseManager.DataReference.Child("UserData").Child(uid);

        _userRef.ValueChanged += OnPlayerDataChanged;
    }

    private void StopListeningToPlayerData()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        _userRef = FirebaseManager.DataReference.Child("UserData").Child(uid);

        _userRef.ValueChanged -= OnPlayerDataChanged;
    }

    private void OnPlayerDataChanged(object sender, ValueChangedEventArgs changeEvent)
    {
        if (changeEvent.DatabaseError != null)
        {
            Debug.LogError($"[DB ERROR] {changeEvent.DatabaseError.Message}");
            return;
        }

        DataSnapshot snapshot = changeEvent.Snapshot;
        
        Debug.Log("데이터 변경");

        PlayerData data = new PlayerData
        {
            PlayerUid = FirebaseManager.Auth.CurrentUser.UserId,
            PlayerName = snapshot.Child("Nickname").Value?.ToString() ?? "LoadFailed",
            Gold = int.TryParse(snapshot.Child("Gold").Value?.ToString(), out int gold) ? gold : 0,
            Diamond = int.TryParse(snapshot.Child("Diamond").Value?.ToString(), out int diamond) ? diamond : 0
        };

        Debug.Log($"[OnPlayerDataChanged] UI 업데이트 / UID: {FirebaseManager.Auth.CurrentUser.UserId}");
        Debug.Log(snapshot.Child("Nickname").Value);
        Debug.Log(snapshot.Child("Gold").Value);
        Debug.Log(snapshot.Child("Diamond").Value);

        _data = data;
        _view.UpdateUI(data);
        OnPlayerProfilePopupUpdated?.Invoke(data);
    }
}
