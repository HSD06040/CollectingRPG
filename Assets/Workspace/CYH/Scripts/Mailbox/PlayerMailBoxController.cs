using System;
using UnityEngine;

public class PlayerMailBoxController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Init();
        }
    }

    // 서버시간 로드 테스트
    private async void LoadServerTime()
    {
        long currentTime = await Manager.DB.LoadSeverTimeAsync();
        Debug.Log($"서버 시간 로드: {DateTimeOffset.FromUnixTimeMilliseconds(currentTime).UtcDateTime.ToLocalTime()}");
        Debug.Log($"서버 시간 로드: {currentTime}");
    }

    // Mail DB 복사 테스트
    private async void Init()
    {
        await Manager.DB.SyncMailsOnLoginAsync();
    }
}
