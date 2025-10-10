using Cysharp.Threading.Tasks;
using Firebase.Database;
using UnityEngine;

public class MagicStoneDB
{
    private DatabaseReference _characterReference;
    private string _uid => FirebaseManager.Auth.CurrentUser.UserId;

    public void EventHandler()
    {
        FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("MagicStoneData").
            ChildChanged += UpdateMagicStoneDatas;
    }

    public async UniTask SaveAllMagicStoneDatas()
    {
        _characterReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("MagicStoneData");

        foreach (var charData in Manager.Data.MagicStones)
        {
            await SaveMagicStoneUpgradeData(charData);
        }
    }

    public async UniTask SaveMagicStoneUpgradeData(MagicStone charData)
    {
        _characterReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("MagicStoneData");

        await _characterReference.Child(charData.Name).
                SetRawJsonValueAsync(JsonUtility.ToJson(charData.UpgradeData.CurrentUpgradeData));
    }

    public async UniTask LoadAllMagicStoneDatas()
    {
        _characterReference = FirebaseManager.DataReference.Child("UserData").Child(_uid).Child("MagicStoneData");
        var dataSnapshot = await _characterReference.GetValueAsync();

        if (dataSnapshot.Exists)
        {
            foreach (var child in dataSnapshot.Children)
            {
                string charName = child.Key;
                UnitData data = Manager.Data.GetUnitData(charName);

                if (data != null)
                {
                    var loaded = JsonUtility.FromJson<CurrentUpgradeData>(child.GetRawJsonValue());
                    data.UpgradeData.CurrentUpgradeData = loaded ?? new CurrentUpgradeData { CurrentPieces = 0, UpgradeLevel = 0 };
                }
            }
        }
    }

    public void UpdateMagicStoneDatas(object sender, ChildChangedEventArgs args)
    {
        if (args.Snapshot.Exists)
        {
            string magicStoneName = args.Snapshot.Key;
            var magicStoneData = Manager.Data.GetMagicStoneData(magicStoneName);

            if (magicStoneData != null)
            {
                int currentPieces = args.Snapshot.Child("CurrentPieces").Value is long pieces
                    ? (int)pieces : 0;
                int upgradeLevel = args.Snapshot.Child("UpgradeLevel").Value is long level
                    ? (int)level : 0;

                magicStoneData.UpgradeData.CurrentUpgradeData.SetData(currentPieces, upgradeLevel);
            }
        }
    }
}