using Cysharp.Threading.Tasks;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDB
{
    private DatabaseReference _characterReference;
    private string _uid => FirebaseManager.Auth.CurrentUser.UserId;

    public void EventHandler()
    {
        FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("CharacterData").
            ChildChanged += UpdateCharacterDatas;
        Debug.Log("캐릭터 데이터 연동됨");
    }

    public async UniTask SaveAllCharacterDatas()
    {
        _characterReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("CharacterData");

        foreach (var charData in Manager.Data.UnitDataDic.Values)
        {
            await SaveCharacterData(charData);
        }
    }

    public async UniTask SaveCharacterData(UnitData charData)
    {
        await _characterReference.Child(charData.Name).
                SetRawJsonValueAsync(JsonUtility.ToJson(charData.UpgradeData.CurrentUpgradeData));
    }

    public async UniTask LoadAllCharacterDatas()
    {
        _characterReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("CharacterData");
        var dataSnapshot = await _characterReference.GetValueAsync();

        if (dataSnapshot.Exists)
        {
            foreach (var child in dataSnapshot.Children)
            {
                string charName = child.Key;
                UnitData data = Manager.Data.GetUnitData(charName);

                if (data != null)
                {
                    data.UpgradeData.CurrentUpgradeData =
                        JsonUtility.FromJson<CurrentUpgradeData>(child.GetRawJsonValue());
                }
            }
        }
    }

    public void UpdateCharacterDatas(object sender, ChildChangedEventArgs args)
    {
        if (args.Snapshot.Exists)
        {
            string charName = args.Snapshot.Key;
            var charData = Manager.Data.GetUnitData(charName);

            if (charData != null)
            {
                int currentPieces = args.Snapshot.Child("CurrentPieces").Value is long pieces
                    ? (int)pieces : 0;
                int upgradeLevel = args.Snapshot.Child("UpgradeLevel").Value is long level
                    ? (int)level : 0;

                charData.UpgradeData.CurrentUpgradeData.SetData(currentPieces, upgradeLevel);
            }
        }
    }
}