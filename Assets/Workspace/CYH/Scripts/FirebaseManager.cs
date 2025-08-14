using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Google;
using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseApp app;
    public static FirebaseApp App { get { return app; } }

    private static FirebaseAuth auth;
    public static FirebaseAuth Auth { get { return auth; } }

    private static FirebaseUser user;
    public static FirebaseUser User { get { return user; } }

    private static FirebaseDatabase database;
    public static FirebaseDatabase Database { get { return database; } }

    private static DatabaseReference dataReference;
    public static DatabaseReference DataReference { get { return dataReference; } }

    // firebase 초기화 완료 여부 체크 플래그
    private bool _isFirebaseReady = false;

    public bool IsFirebaseReady
    {
        get { return _isFirebaseReady; }
        private set { _isFirebaseReady = value; }
    }

    private void Start()
    {
        InitFirebase();
    }

    private void InitFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
                Debug.Log("설정 완료");
            }
            else
            {
                app = null;
                auth = null;
                database = null;
            }
        });
    }
}