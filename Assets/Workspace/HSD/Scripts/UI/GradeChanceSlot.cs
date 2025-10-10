using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GradeChanceSlot : MonoBehaviour
{
    [SerializeField] private Image _gradeImage;
    [SerializeField] private TMP_Text _chanceText;

    public void SetGradeChance(Grade grade, float chance)
    {
        _gradeImage.color = grade.GetGradeColor();
        _chanceText.text = $"{chance.ToString("F1")}%";
    }
}
