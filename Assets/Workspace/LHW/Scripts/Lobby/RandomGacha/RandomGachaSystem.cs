using System;
using UnityEngine;
using UnityEngine.UI;

public class RandomGachaSystem : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CharacterDatabase _data;
    [SerializeField] private ItemProbabilitySO _prob;
    [SerializeField] private GachaResultUI _resultUI;

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
        _dailyButton.onClick.AddListener(FreeOrAdButtonClick);
        _oneGachaButton.onClick.AddListener(() => ConsumeGoodsButtonClick(1));
        _tenGachaButton.onClick.AddListener(() => ConsumeGoodsButtonClick(10));
    }

    private void RandomInit(ItemProbabilitySO probability)
    {
        for (int i = 0; i < probability.ItemsProbability.Count; i++)
        {
            Grade grade = probability.ItemsProbability[i].ItemGrade;
            int value = (int)(probability.ItemsProbability[i].Probability * (int)Math.Pow(10, digits));
            _gradeRandom.Add(grade, value);
        }
    }

    // 확률 변동 없는 캐릭터 뽑기
    private UnitData ReturnData()
    {
        Grade grade = _gradeRandom.GetRandomItem();
        return _data.GetRandomUnitByGrade(grade);
    }

    // 확률 변동 있는 캐릭터 뽑기
    private UnitData ReturnDataBySub()
    {
        Grade grade = _gradeRandom.GetRandomItemBySub();
        return _data.GetRandomUnitByGrade(grade);
    }

    private void FreeOrAdButtonClick()
    {
        // 일일 뽑기가 가능한지 확인
        // 가능하면 ItemSelect(1) 진행
        // 불가능할 시 광고 뽑기 진행 여부 확인
        if (DailyFreeGacha()) return;
        // 광고 뽑기가 가능한지 확인
        // 가능하면 ItemSelect(1) 진행
        // 불가능할 시 Return(경고팝업 띄우기)
        if(DailyAdGacha()) return;

        if(PopupManager.Instance != null)
        {
            PopupManager.instance.ShowPopup("일일 무료 가챠를 전부 사용하였습니다.");
        }
    }

    private bool DailyFreeGacha()
    {
        if (TimeManager.Instance.CanObtainedFreeGachaReward())
        {
            TimeManager.Instance.SaveDailyFreeGachaResetTimeInfo();
            ItemSelect(1);
            return true;
        }
        return false;
    }

    private bool DailyAdGacha()
    {
        if(TimeManager.Instance.CanObtainAdGachaReward())
        {
            TimeManager.Instance.SaveAdGachaResetTimeInfo();
            ItemSelect(1);
            return true;
        }

        return false;
    }

    private void ConsumeGoodsButtonClick(int number)
    {
        // 재화 상태 확인 절차 진행

        ItemSelect(number);
    }

    // 확률 변동이 없는 가중치 확률
    private void ItemSelect(int number)
    {
        if (_gradeRandom.GetList() == null) RandomInit(_prob);

        for (int i = 0; i < number; i++)
        {
            UnitData data = ReturnData();
            _resultUI.HeroGachaUpdate(data, i);
        }

        _resultUI.gameObject.SetActive(true);
    }



    // 천장이 있는 가중치 확률
    private void ItemSelectBySub(int number)
    {
        if (_gradeRandom.GetList() == null) RandomInit(_prob);

        for (int i = 0; i < number; i++)
        {
            ReturnDataBySub();
        }
    }
}