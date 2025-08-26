using System;
using UnityEngine;
using UnityEngine.UI;

public class RandomGachaSystem : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CharacterDatabase _data;
    [SerializeField] private ItemProbabilitySO _prob;

    [Header("UI")]
    [SerializeField] private Button _dailyButton;
    [SerializeField] private Button _oneGachaButton;
    [SerializeField] private Button _tenGachaButton;

    // 확률 소수점 자릿수
    [Header("ProbOffset")]
    [SerializeField] private int digits = 0;

    private WeightedRandom<Grade> _gradeRandom = new WeightedRandom<Grade>();

    private void Awake()
    {
        RandomInit(_prob);
        _dailyButton.onClick.AddListener(() => ItemSelect(1));
        _oneGachaButton.onClick.AddListener(() => ItemSelect(1));
        _tenGachaButton.onClick.AddListener(() => ItemSelect(10));
    }

    private void RandomInit(ItemProbabilitySO probability)
    {        
        for(int i = 0; i < probability.ItemsProbability.Count; i++)
        {
            Grade grade = probability.ItemsProbability[i].ItemGrade;
            int value = (int)(probability.ItemsProbability[i].Probability * (int)Math.Pow(10, digits));
            _gradeRandom.Add(grade, value);
        }
    }

    private void ReturnData()
    {
        Grade grade = _gradeRandom.GetRandomItem();

        UnitData unit = _data.GetRandomUnitByGrade(grade);

        Debug.Log($"뽑힌 등급: {grade}, 캐릭터: {unit?.Name}");
    }

    private void ReturnDataBySub()
    {
        // grade에 해당하는 캐릭터를 랜덤으로 뽑기
        Grade grade = _gradeRandom.GetRandomItemBySub();

        UnitData unit = _data.GetRandomUnitByGrade(grade);

        Debug.Log($"뽑힌 등급: {grade}, 캐릭터: {unit?.Name}");
    }

    // 확률 변동이 없는 가중치 확률
    private void ItemSelect(int number)
    {
        if (_gradeRandom.GetList() == null) RandomInit(_prob);

        for(int i = 0; i < number; i++)
        {
            ReturnData();
        }
    }

    // 천장이 있는 가중치 확률
    private void ItemSelectBySub(int number)
    {
        if (_gradeRandom.GetList() == null) RandomInit(_prob);

        for(int i = 0; i < number; i++)
        {
            ReturnDataBySub();
        }
    }
}