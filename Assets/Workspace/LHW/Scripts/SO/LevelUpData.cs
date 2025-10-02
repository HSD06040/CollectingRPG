using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unit_LevelUpData", menuName = "Data/Temp/Unit_LevelUpData")]
public class LevelUpData : ScriptableObject
{
    public List<RequirePiece> RequirePieceData = new();

    /// <summary>
    /// 현재 캐릭터의 등급, 레벨을 기반으로, 최대 레벨을 찍기까지
    /// 남은 조각 개수를 반환하는 함수
    /// </summary>
    /// <param name="grade"></param>
    /// <param name="level"></param>
    public int GetCumulativePiece(Grade grade, int level)
    {
        RequirePiece requirePiece = RequirePieceData.Find(r => r.Grade == grade);
        if (requirePiece == null)
        {
            Debug.LogError($"[{name}] {grade} 등급에 맞는 RequirePiece 데이터가 없습니다.");
            return 0;
        }

        int cumulativePiece = 0;

        int index = level - 1;
        if (index < 0)
        {
            index = 0;
            cumulativePiece += 10;
        }

        for (int i = index; i < requirePiece.LevelRatio.Count; i++)
        {
            cumulativePiece += requirePiece.LevelRatio[i].RequirePiece;
        }
        Debug.Log(cumulativePiece);

        return cumulativePiece;
    }
}
