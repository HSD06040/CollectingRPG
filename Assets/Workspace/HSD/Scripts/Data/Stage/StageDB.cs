using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using System.Collections.Generic;

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

    public async UniTask SaveStageClearData(StageData stageData, int stageNumber, bool cleared = true)
    {
        _stageDataReference = GetStageReference(stageData, stageNumber);

        var clearData = new StageClearData { isCleared = cleared };

        await _stageDataReference.SetRawJsonValueAsync(JsonUtility.ToJson(clearData));

        Debug.Log($"스테이지 클리어 데이터 저장 완료: 지역 {stageData.RegionNumber}, 스테이지 {stageNumber}, 클리어 여부: {cleared}");
    }

    public async UniTask LoadAllStageClearDatas()
    {
        _stageDataReference = GetBaseStageReference();
        var dataSnapshot = await _stageDataReference.GetValueAsync();

        if (dataSnapshot.Exists)
        {
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
                    Debug.LogError($"지역 번호 {regionNumber}에 해당하는 로컬 StageData를 찾을 수 없습니다. 데이터 로드 실패.");
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

                    var loadedData = JsonUtility.FromJson<StageClearData>(stageChild.GetRawJsonValue());

                    if (loadedData != null)
                    {
                        localStageData.SetClearProof(stageNumber, loadedData.isCleared);
                        Debug.Log($"DB 데이터 로드 후 로컬 클리어 증명 세팅 완료: 지역 {regionNumber}, 스테이지 {stageNumber}, 클리어 여부: {loadedData.isCleared}");
                    }
                }
            }
        }
        else
        {
            Debug.Log("이 사용자에게 저장된 스테이지 클리어 데이터가 없습니다. 모든 스테이지를 미클리어 상태로 간주합니다.");
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
        if (args.Snapshot.Exists)
        {
            string regionKey = args.Snapshot.Key;
            if (!int.TryParse(regionKey.Replace("Stage", ""), out int regionNumber)) return;

            var localStageData = Manager.Data.StageDatas.GetStage(regionNumber);
            if (localStageData == null)
            {
                Debug.LogError($"실시간 업데이트 중 지역 번호 {regionNumber}에 해당하는 로컬 StageData를 찾을 수 없습니다.");
                return;
            }


            foreach (var stageChild in args.Snapshot.Children)
            {
                if (!int.TryParse(stageChild.Key, out int stageNumber)) continue;

                var loadedData = JsonUtility.FromJson<StageClearData>(stageChild.GetRawJsonValue());

                if (loadedData != null)
                {
                    localStageData.SetClearProof(stageNumber, loadedData.isCleared);
                    Debug.Log($"스테이지 데이터 실시간 업데이트됨: 지역 {regionNumber}, 스테이지 {stageNumber}, 클리어 여부: {loadedData.isCleared}");
                }
            }
        }
    }
}