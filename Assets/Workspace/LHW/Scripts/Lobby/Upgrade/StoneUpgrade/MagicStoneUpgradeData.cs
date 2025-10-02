using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStone_UpgradeMagicStoneData", menuName = "Data/Upgrade/MagicStone_UpgradeMagicStoneData")]
public class MagicStoneUpgradeData : ScriptableObject
{
    // 캐릭터의 업그레이드 레벨 - 레벨이 0일 때는 획득하지 않은 상태
    private SubGrade _grade;
    private MagicStoneLevelUpData _levelUpData;

    public CurrentUpgradeData CurrentUpgradeData;

    public event Action OnLevelUp;

    public void Init(SubGrade grade, MagicStoneLevelUpData data)
    {
        _grade = grade;
        _levelUpData = data;
    }

    public int GetRequiredPiece()
    {
        if (CurrentUpgradeData.UpgradeLevel >= 10 ||
            CurrentUpgradeData.UpgradeLevel <= 0) return 0;

        if (_levelUpData == null)
        {
            Debug.LogError($"[{name}] LevelUpData가 설정되지 않았습니다.");
            return 0;
        }

        PieceLevelRatio pieceLevelRatio = _levelUpData.LevelRatio.Find(l => l.Level == CurrentUpgradeData.UpgradeLevel + 1);
        if (pieceLevelRatio == null)
        {
            Debug.LogError($"[{name}] {_grade} / {CurrentUpgradeData.UpgradeLevel}에 맞는 PieceLevelRatio 데이터가 없습니다.");
            return 0;
        }

        return pieceLevelRatio.RequirePiece;
    }

    public int GetRequiredGold()
    {
        if (CurrentUpgradeData.UpgradeLevel >= 10 ||
            CurrentUpgradeData.UpgradeLevel <= 0) return 0;

        if (_levelUpData == null)
        {
            Debug.LogError($"[{name}] LevelUpData가 설정되지 않았습니다.");
            return 0;
        }

        PieceLevelRatio pieceLevelRatio = _levelUpData.LevelRatio.Find(l => l.Level == CurrentUpgradeData.UpgradeLevel + 1);
        if (pieceLevelRatio == null)
        {
            Debug.LogError($"[{name}] {_grade} / {CurrentUpgradeData.UpgradeLevel}에 맞는 PieceLevelRatio 데이터가 없습니다.");
            return 0;
        }

        return pieceLevelRatio.RequireGold;
    }

    public void AddPiece(int piece)
    {
        CurrentUpgradeData.CurrentPieces += piece;
    }

    public async Task<bool> LevelUpWithPiecesOnly()
    {
        if (CurrentUpgradeData.UpgradeLevel >= 10) return false;

        if (CurrentUpgradeData.UpgradeLevel <= 0)
        {
            if (CurrentUpgradeData.CurrentPieces >= 10)
            {
                CurrentUpgradeData.CurrentPieces -= 10;
                CurrentUpgradeData.UpgradeLevel += 1;
                return true;
            }
            else
            {
                if (PopupManager.Instance != null)
                {
                    PopupManager.instance.ShowPopup("조각이 부족합니다.");
                }
                return false;
            }
        }
        else if (CurrentUpgradeData.UpgradeLevel >= 1)
        {
            int requiredPiece = GetRequiredPiece();
            int requiredGold = GetRequiredGold();

            // 골드, 조각 체크
            string uid = FirebaseManager.Auth.CurrentUser.UserId;
            var goldRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("Gold");
            DataSnapshot snapshot = await goldRef.GetValueAsync();
            int currentGold = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;

            if (CurrentUpgradeData.CurrentPieces >= requiredPiece && currentGold >= requiredGold)
            {
                CurrentUpgradeData.CurrentPieces -= requiredPiece;
                CurrentUpgradeData.UpgradeLevel += 1;

                await DBManager.Instance.SubtractGoldAsync(requiredGold);
                OnLevelUp?.Invoke();
                return true;
            }
        }
        return false;
    }

    public async Task<bool> CanLevelUpWithMythStone()
    {

        if (CurrentUpgradeData.UpgradeLevel >= 10) return false;

        int requiredPiece = GetRequiredPiece();
        int requiredGold = GetRequiredGold();

        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        var goldRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("Gold");
        var mythStoneRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("MagicStone");

        DataSnapshot goldSnap = await goldRef.GetValueAsync();
        DataSnapshot mythSnap = await mythStoneRef.GetValueAsync();

        int currentGold = goldSnap.Exists ? Convert.ToInt32(goldSnap.Value) : 0;
        int currentMythStone = mythSnap.Exists ? Convert.ToInt32(mythSnap.Value) : 0;

        int ratio = MythStonePieceRatio(_grade);
        int conversedMythstone = currentMythStone / ratio;

        return (CurrentUpgradeData.CurrentPieces + conversedMythstone >= requiredPiece
            && currentGold >= requiredGold);
    }

    public async Task LevelUpWithMythStone()
    {

        int requiredPiece = GetRequiredPiece();
        int requiredGold = GetRequiredGold();
        int ratio = MythStonePieceRatio(_grade);

        int neededFromMythStone = requiredPiece - CurrentUpgradeData.CurrentPieces;
        if (neededFromMythStone > 0)
            await DBManager.Instance.SubtractMythStoneAsync(neededFromMythStone * ratio);

        CurrentUpgradeData.CurrentPieces = 0;
        CurrentUpgradeData.UpgradeLevel += 1;
        await DBManager.Instance.SubtractGoldAsync(requiredGold);

        OnLevelUp?.Invoke();
    }

    private int MythStonePieceRatio(SubGrade grade)
    {
        int pieceRatio = 0;

        switch (grade)
        {
            case SubGrade.SILVER: pieceRatio = 2; break;
            case SubGrade.GOLD: pieceRatio = 4; break;
            case SubGrade.PRISM: pieceRatio = 6; break;
        }

        return pieceRatio;
    }
}

[CreateAssetMenu(fileName = "MagicStone_LevelUpData", menuName = "Data/Temp/MagicStone_LevelUpData")]
public class MagicStoneLevelUpData : ScriptableObject
{
    public List<PieceLevelRatio> LevelRatio = new List<PieceLevelRatio>();

    public int GetCumulativePiece(int level)
    {
        int cumulativePiece = 0;

        int index = level - 1;
        if (index < 0)
        {
            index = 0;
            cumulativePiece += 10;
        }

        for (int i = index; i < LevelRatio.Count; i++)
        {
            cumulativePiece += LevelRatio[i].RequirePiece;
        }
        Debug.Log(cumulativePiece);

        return cumulativePiece;
    }
}