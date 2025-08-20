using UnityEngine;
using Firebase.Database;

public class PlayerDataController : MonoBehaviour
{
    [SerializeField] private PlayerDataView _view;
    private DatabaseReference _userRef;


    private void Start()
    {
        InitAsync();
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
        PlayerData data = await DBManager.Instance.LoadLobbyDataAsync();
        _view.UpdateUI(data);
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

        Debug.Log(snapshot.Child("Nickname").Value);
        Debug.Log(snapshot.Child("Gold").Value);
        Debug.Log(snapshot.Child("Diamond").Value);

        _view.UpdateUI(data);
    }
}
