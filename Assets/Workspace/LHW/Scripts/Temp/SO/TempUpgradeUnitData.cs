using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_TempUpgradeUnitData", menuName = "Data/Temp/Unit_TempUpgradeUnitData")]
public class TempUpgradeUnitData : ScriptableObject
{
    // 해당 캐릭터 등급 -> 이후 UnitData에서 직접 참조하는 방식으로 변경
    public Grade Grade;
    // 캐릭터의 획득 여부
    public bool IsCollected;
    // 캐릭터의 업그레이드 레벨
    public int UpgradeLevel;
    // 현재 보유 캐릭터 조각 수
    public int CurrentPieces;

    // 캐릭터 강화 요구 조각 수 데이터 -> 이후 UnitData로 옮기는 방법 고민중
    [field:SerializeField] public LevelUpData LevelUpData {  get; private set; }   
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

[Serializable]
public class PieceLevelRatio
{
    public int Level;
    public int RequirePiece;
}