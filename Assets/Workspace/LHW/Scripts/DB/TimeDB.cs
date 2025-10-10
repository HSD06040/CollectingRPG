using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using UnityEngine;

public class TimeDB
{
    private DatabaseReference _timeReference;
    private string _uid => FirebaseManager.Auth.CurrentUser.UserId;

    public async UniTask SaveTimeData(GachaType type, bool isAd, RewardInfo info)
    {
        _timeReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("TimeData");

        string path = GetPath(type, isAd);

        await _timeReference.Child(path).SetRawJsonValueAsync(JsonUtility.ToJson(info));
    }

    public async UniTask<RewardInfo> LoadGachaTime(GachaType type, bool isAd)
    {
        _timeReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("TimeData");
        string path = GetPath(type, isAd);
        var snapshot = await _timeReference.Child(path).GetValueAsync();

        if (!snapshot.Exists)
        {
            Debug.LogWarning($"⚠️ {path} 데이터가 없음. 기본값으로 생성.");

            RewardInfo newRewardInfo;

            if (isAd)
            {
                newRewardInfo = new RewardInfo(DateTime.Now.Ticks, 2);
                await SaveTimeData(type, isAd, newRewardInfo);
            }
            else
            {
                newRewardInfo = new RewardInfo(DateTime.Now.Ticks, 1);
                await SaveTimeData(type, isAd, newRewardInfo);
            }
            return newRewardInfo;
        }

        return JsonUtility.FromJson<RewardInfo>(snapshot.GetRawJsonValue());
    }

    private string GetPath(GachaType type, bool isAd)
    {
        string typeStr = type == GachaType.Char ? "CharGacha" : "StoneGacha";
        string adStr = isAd ? "Ad" : "Free";
        return $"{typeStr}/{adStr}";
    }
}
