using Cysharp.Threading.Tasks;
using Firebase.Database;
using System;
using UnityEngine;

public class TimeDB
{
    private DatabaseReference _timeReference;
    private string _uid => FirebaseManager.Auth.CurrentUser.UserId;

    /// <summary>
    /// 가챠 시간 저장
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isAd"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public async UniTask SaveTimeData(GachaType type, bool isAd, RewardInfo info)
    {
        _timeReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("TimeData");

        string path = GetPath(type, isAd);

        await _timeReference.Child(path).SetRawJsonValueAsync(JsonUtility.ToJson(info));
    }

    public async UniTask SaveShopResetTime(DateTime resetTime)
    {
        _timeReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("TimeData");

        await _timeReference.Child("Shop").Child("dateTicks").SetValueAsync(resetTime.Ticks);
    }

    /// <summary>
    /// 가챠 시간 로드
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isAd"></param>
    /// <returns></returns>
    public async UniTask<RewardInfo> LoadTimeData(GachaType type, bool isAd)
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

    public async UniTask<DateTime> LoadShopResetTime()
    {
        _timeReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("TimeData");
        var snapshot = await _timeReference.Child("Shop").Child("dateTicks").GetValueAsync();

        DateTime resetTime;

        if (!snapshot.Exists)
        {
            DateTime now = DateTime.Now;
            DateTime todayReset = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);

            resetTime = (now.Hour < 6) ? todayReset : todayReset.AddDays(1);

            await SaveShopResetTime(resetTime);
            return resetTime;
        }

        long ticks = long.Parse(snapshot.Value.ToString());
        return new DateTime(ticks);
    }

    private string GetPath(GachaType type, bool isAd)
    {
        string typeStr = type == GachaType.Char ? "CharGacha" : "StoneGacha";
        string adStr = isAd ? "Ad" : "Free";
        return $"{typeStr}/{adStr}";
    }
}
