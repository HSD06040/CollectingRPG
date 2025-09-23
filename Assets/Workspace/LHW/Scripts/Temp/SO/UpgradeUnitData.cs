using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_UpgradeUnitData", menuName = "Data/Upgrade/Unit_UpgradeUnitData")]
public class UpgradeUnitData : ScriptableObject
{
    // 캐릭터의 업그레이드 레벨 - 레벨이 0일 때는 획득하지 않은 상태
    private Grade _grade;
    private LevelUpData _levelUpData;

    public CurrentUpgradeData CurrentUpgradeData;

    public void Init(Grade grade, LevelUpData data)
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

        RequirePiece requirePiece = _levelUpData.RequirePieceData.Find(r=>r.Grade == _grade);
        if (requirePiece == null)
        {
            Debug.LogError($"[{name}] {_grade} 등급에 맞는 RequirePiece 데이터가 없습니다.");
            return 0;
        }

        PieceLevelRatio pieceLevelRatio = requirePiece.LevelRatio.Find(l => l.Level == CurrentUpgradeData.UpgradeLevel + 1);
        if (pieceLevelRatio == null)
        {
            Debug.LogError($"[{name}] {_grade} / {CurrentUpgradeData.UpgradeLevel}에 맞는 PieceLevelRatio 데이터가 없습니다.");
            return 0;
        }

        return pieceLevelRatio.RequirePiece;
    }

    public void ObtainCharacter()
    {
        if (CurrentUpgradeData.UpgradeLevel == 0) CurrentUpgradeData.UpgradeLevel += 1;

    }

    public void AddPiece(int piece)
    {
        CurrentUpgradeData.CurrentPieces += piece;
    }

    public void LevelUp()
    {
        // 최대레벨 변수 추가?
        if (CurrentUpgradeData.UpgradeLevel >= 10 || CurrentUpgradeData.UpgradeLevel <= 0) return;

        int requiredPiece = GetRequiredPiece();
        
        if(CurrentUpgradeData.CurrentPieces >= requiredPiece)
        {
            CurrentUpgradeData.CurrentPieces -= requiredPiece;
            CurrentUpgradeData.UpgradeLevel += 1;
        }
    }    
}

[CreateAssetMenu(fileName = "Unit_LevelUpData", menuName = "Data/Temp/Unit_LevelUpData")]
public class LevelUpData : ScriptableObject
{
    public List<RequirePiece> RequirePieceData = new();
}

[Serializable]
public class RequirePiece
{
    public Grade Grade;
    public List<PieceLevelRatio> LevelRatio = new List<PieceLevelRatio>();
}

/// <summary>
/// 에디터상 입력을 위해 임시로 List 로 처리함. 추후에 Dictionary로 전환할 필요성 있음
/// </summary>
[Serializable]
public class PieceLevelRatio
{
    public int RequirePiece;
    public int Level;
}

[Serializable]
public class CurrentUpgradeData
{
    // 현재 보유 캐릭터 조각 수
    public int CurrentPieces;

    public int UpgradeLevel;

    public void SetData(int currentPieces, int upgradeLevel)
    {
        CurrentPieces = currentPieces;
        UpgradeLevel = upgradeLevel;
    }
}