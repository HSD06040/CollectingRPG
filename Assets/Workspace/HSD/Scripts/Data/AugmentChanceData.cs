using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AugmentChanceData", menuName = "Data/Augment/ChanceData")]
public class AugmentChanceData : ScriptableObject
{
    [SerializeField] SubGradeChanceData[] _subGradeChanceDatas;
    private static readonly Dictionary<SubGrade, float> _subGradeChanceDic = new Dictionary<SubGrade, float>();

    public AUGData GetAugmentData()
    {
        if (_subGradeChanceDic.Count == 0)
            Init();

        SubGrade grade = GetRandomGrade();

        AUGData selected = TryGetAugDataByGrade(grade);

        if (selected == null)
        {
            foreach (var otherGrade in _subGradeChanceDic.Keys)
            {
                selected = TryGetAugDataByGrade(otherGrade);
                if (selected != null)
                    break;
            }
        }

        if (selected == null)
        {
            Debug.LogError("No AUGData found Check Manager.Data.AugmentDatas");
        }

        return selected;
    }

    private AUGData TryGetAugDataByGrade(SubGrade grade)
    {
        AUGData[] augmentDatas = Array.FindAll(Manager.Data.AugmentDatas, a => a.Grade == grade);

        var available = augmentDatas
            .Where(a => !AugmentManager.Instance.currentAugment.Contains(a))
            .ToArray();

        if (available.Length == 0)
            return null;

        return available[UnityEngine.Random.Range(0, available.Length)];
    }

    private void Init()
    {
        for (int i = 0; i < _subGradeChanceDatas.Length; i++)
        {
            var data = _subGradeChanceDatas[i];

            if (!_subGradeChanceDic.ContainsKey(data.Grade))
            {
                _subGradeChanceDic.Add(data.Grade, data.Chance);
            }
        }
    }

    private SubGrade GetRandomGrade()
    {
        float totalChance = _subGradeChanceDic.Values.Sum();
        float randomValue = UnityEngine.Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var kvp in _subGradeChanceDic)
        {
            cumulative += kvp.Value;
            if (randomValue <= cumulative)
                return kvp.Key;
        }

        return _subGradeChanceDic.Keys.Last();
    }
}
