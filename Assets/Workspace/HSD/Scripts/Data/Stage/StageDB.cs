using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StageProgressData
{
    public bool isCleared;
    public bool isFirstRewardGained;
}

public class StageDB
{
    private DatabaseReference _stageDataReference;
    private string _uid => FirebaseManager.Auth.CurrentUser.UserId;
    private const string StageDataPath = "StageData";

    private DatabaseReference GetBaseStageReference()
    {
        return FirebaseManager.DataReference
            .Child("UserData").Child(_uid).Child(StageDataPath);
    }

    private DatabaseReference GetStageReference(StageData stageData, int stageNumber)
    {
        return GetBaseStageReference()
            .Child($"Stage{stageData.RegionNumber}")
            .Child(stageNumber.ToString());
    }

    public async UniTask SaveStageProgress(StageData stageData, int stageNumber, bool cleared = true, bool firstRewardGained = false)
    {
        _stageDataReference = GetStageReference(stageData, stageNumber);

        var progressData = new StageProgressData
        {
            isCleared = cleared,
            isFirstRewardGained = firstRewardGained
        };

        await _stageDataReference.SetRawJsonValueAsync(JsonUtility.ToJson(progressData));

        Debug.Log($"스테이지 진행 데이터 저장 완료: 지역 {stageData.RegionNumber}, 스테이지 {stageNumber}, 클리어 여부: {cleared}, 최초보상: {firstRewardGained}");
    }

    public async UniTask SaveStageFirstRewardGainData(StageData stageData, int stageNumber, bool isGained = true)
    {
        _stageDataReference = GetStageReference(stageData, stageNumber);

        var snapshot = await _stageDataReference.GetValueAsync();
        StageProgressData progressData;

        if (snapshot.Exists)
            progressData = JsonUtility.FromJson<StageProgressData>(snapshot.GetRawJsonValue());
        else
            progressData = new StageProgressData();

        progressData.isFirstRewardGained = isGained;

        await _stageDataReference.SetRawJsonValueAsync(JsonUtility.ToJson(progressData));

        Debug.Log($"최초 보상 데이터 업데이트 완료: 지역 {stageData.RegionNumber}, 스테이지 {stageNumber}, 보상 획득: {isGained}");
    }

    public async UniTask LoadAllStageClearDatas()
    {
        _stageDataReference = GetBaseStageReference();
        var dataSnapshot = await _stageDataReference.GetValueAsync();

        if (!dataSnapshot.Exists)
        {
            Debug.Log("이 사용자에게 저장된 스테이지 데이터가 없습니다. 모든 스테이지를 미클리어 상태로 간주합니다.");
            return;
        }

        foreach (var regionChild in dataSnapshot.Children)
        {
            string regionKey = regionChild.Key;
            if (!int.TryParse(regionKey.Replace("Stage", ""), out int regionNumber))
            {
                Debug.LogWarning($"유효하지 않은 지역 키 건너뜀: {regionKey}");
                continue;
            }

            var localStageData = Manager.Data.StageDatas.GetStage(regionNumber);
            if (localStageData == null)
            {
                Debug.LogError($"지역 번호 {regionNumber}에 해당하는 로컬 StageData를 찾을 수 없습니다.");
                continue;
            }

            foreach (var stageChild in regionChild.Children)
            {
                string stageNumberStr = stageChild.Key;
                if (!int.TryParse(stageNumberStr, out int stageNumber))
                {
                    Debug.LogWarning($"유효하지 않은 스테이지 번호 키 건너뜀: {stageNumberStr}");
                    continue;
                }

                var loadedData = JsonUtility.FromJson<StageProgressData>(stageChild.GetRawJsonValue());
                if (loadedData != null)
                {
                    localStageData.SetClearProof(stageNumber, loadedData.isCleared);
                    localStageData.SetFirstRewardGainProof(stageNumber, loadedData.isFirstRewardGained);

                    Debug.Log($"데이터 로드 완료: 지역 {regionNumber}, 스테이지 {stageNumber}, 클리어:{loadedData.isCleared}, 보상:{loadedData.isFirstRewardGained}");
                }
            }
        }
    }

    public void EventHandler()
    {
        FirebaseManager.DataReference.Child("UserData").Child(_uid).Child(StageDataPath)
            .ChildChanged += UpdateStageClearDatas;
        Debug.Log("스테이지 데이터 실시간 리스너 연결됨");
    }

    public void UpdateStageClearDatas(object sender, ChildChangedEventArgs args)
    {
        if (!args.Snapshot.Exists) return;

        string regionKey = args.Snapshot.Key;
        if (!int.TryParse(regionKey.Replace("Stage", ""), out int regionNumber)) return;

        var localStageData = Manager.Data.StageDatas.GetStage(regionNumber);
        if (localStageData == null)
        {
            Debug.LogError($"실시간 업데이트 중 지역 {regionNumber}에 해당하는 로컬 StageData를 찾을 수 없습니다.");
            return;
        }

        foreach (var stageChild in args.Snapshot.Children)
        {
            if (!int.TryParse(stageChild.Key, out int stageNumber)) continue;

            var loadedData = JsonUtility.FromJson<StageProgressData>(stageChild.GetRawJsonValue());
            if (loadedData != null)
            {
                localStageData.SetClearProof(stageNumber, loadedData.isCleared);
                localStageData.SetFirstRewardGainProof(stageNumber, loadedData.isFirstRewardGained);

                Debug.Log($"실시간 업데이트: 지역 {regionNumber}, 스테이지 {stageNumber}, 클리어:{loadedData.isCleared}, 보상:{loadedData.isFirstRewardGained}");
            }
        }
    }
}
