using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_TempUpgradeUnitData", menuName = "Data/Temp/Unit_TempUpgradeUnitData")]
public class TempUpgradeUnitData : ScriptableObject
{
    // 해당 캐릭터 등급 -> 이후 UnitData에서 직접 참조하는 방식으로 변경
    public Grade Grade;
    // 캐릭터의 업그레이드 레벨 - 레벨이 0일 때는 획득하지 않은 상태
    public int UpgradeLevel;
    // 현재 보유 캐릭터 조각 수
    public int CurrentPieces;

    // 캐릭터 강화 요구 조각 수 데이터 -> 이후 UnitData로 옮기는 방법 고민중
    [field:SerializeField] public LevelUpData LevelUpData { get; private set; }
    
    public int GetRequiredPiece()
    {
        if (UpgradeLevel >= 10 || UpgradeLevel <= 0) return 0;

        if (LevelUpData == null)
        {
            Debug.LogError($"[{name}] LevelUpData가 설정되지 않았습니다.");
            return 0;
        }

        RequirePiece requirePiece = LevelUpData.RequirePieceData.Find(r => r.Grade == Grade);
        if (requirePiece == null)
        {
            Debug.LogError($"[{name}] {Grade} 등급에 맞는 RequirePiece 데이터가 없습니다.");
            return 0;
        }

        PieceLevelRatio pieceLevelRatio = requirePiece.LevelRatio.Find(l => l.Level == UpgradeLevel + 1);
        if (pieceLevelRatio == null)
        {
            Debug.LogError($"[{name}] {Grade} / {UpgradeLevel}에 맞는 PieceLevelRatio 데이터가 없습니다.");
            return 0;
        }

        return pieceLevelRatio.RequirePiece;
    }

    public void ObtainCharacter()
    {
        if (UpgradeLevel == 0) UpgradeLevel += 1;
    }

    public void AddPiece(int piece)
    {
        CurrentPieces += piece;
        Debug.Log("데이터 변동");
    }

    public void LevelUp()
    {
        // 최대레벨 변수 추가?
        if (UpgradeLevel >= 10 || UpgradeLevel <= 0) return;

        int requiredPiece = GetRequiredPiece();
        
        if(CurrentPieces >= requiredPiece)
        {
            CurrentPieces -= requiredPiece;
            UpgradeLevel += 1;
        }
    }
}

[CreateAssetMenu(fileName = "Unit_LevelUpData", menuName = "Data/Temp/Unit_LevelUpData")]
public class LevelUpData : ScriptableObject
{
    public List<RequirePiece> RequirePieceData = new List<RequirePiece>();
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
    public int Level;
    public int RequirePiece;
}