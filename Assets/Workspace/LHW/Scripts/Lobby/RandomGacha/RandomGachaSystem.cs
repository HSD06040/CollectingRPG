using System;
using UnityEngine;
using UnityEngine.UI;

public class RandomGachaSystem : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private CharacterDatabase _data;
    [SerializeField] private ItemProbabilitySO _prob;
    [SerializeField] private GachaResultUI _resultUI;

    [Header("GachaListUIButton")]
    [SerializeField] private Button _characterGachaButton;
    [SerializeField] private Button _stoneGachaButton;

    [Header("GachaListUI")]
    [SerializeField] private GameObject _characterGacha;
    [SerializeField] private GameObject _stoneGacha;

    [Header("CharacterGachaUI")]
    [SerializeField] private Button _dailyCharacterGachaButton;
    [SerializeField] private Button _oneCharacterGachaButton;
    [SerializeField] private Button _tenCharacterGachaButton;

    [Header("MagicStoneGachaUI")]
    [SerializeField] private Button _dailyStoneButton;
    [SerializeField] private Button _oneStoneGachaButton;
    [SerializeField] private Button _tenStoneGachaButton;

    // 확률 소수점 자릿수
    [Header("ProbOffset")]
    [SerializeField] private int digits = 0;

    private WeightedRandom<Grade> _gradeRandom = new WeightedRandom<Grade>();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        RandomInit(_prob);

        _characterGachaButton.onClick.AddListener(() => SetActivePanel("CharacterGacha"));
        _stoneGachaButton.onClick.AddListener(() => SetActivePanel("MagicStoneGacha"));

        _dailyCharacterGachaButton.onClick.AddListener(AdButtonClick);
        _oneCharacterGachaButton.onClick.AddListener(OneButtonClick);
        _tenCharacterGachaButton.onClick.AddListener(() => ConsumeGoodsButtonClick(10));
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

    #region Button Click Event

    #region GachaListButton

    private void SetActivePanel(string activePanel)
    {
        _characterGacha.SetActive(activePanel.Equals(_characterGacha.name));
        _stoneGacha.SetActive(activePanel.Equals(_stoneGacha.name));
    }

    #endregion

    #region CharacterGachaButton

    private void AdButtonClick()
    {
        if(DailyAdGacha()) return;

        if(PopupManager.Instance != null)
        {
            PopupManager.instance.ShowPopup("일일 광고 가챠를 전부 사용하였습니다.");
        }
    }
    private bool DailyAdGacha()
    {
        if (TimeManager.Instance.CanObtainAdGachaReward())
        {
            TimeManager.Instance.SaveAdGachaResetTimeInfo();
            ItemSelect(1);
            TimeManager.Instance.OnDailyGachaInfoChanged?.Invoke();
            return true;
        }

        return false;
    }

    private void OneButtonClick()
    {
        if (DailyFreeGacha()) return;

        // 재화 소모 일시로 막아둠(신원님 요청)
        //ItemSelect(1);

        if (PopupManager.Instance != null)
        {
            PopupManager.instance.ShowPopup("일일 무료 가챠를 이미 진행하였습니다.");
        }
    }

    private bool DailyFreeGacha()
    {
        if (TimeManager.Instance.CanObtainedFreeGachaReward())
        {
            TimeManager.Instance.SaveDailyFreeGachaResetTimeInfo();
            ItemSelect(1);
            TimeManager.Instance.OnDailyGachaInfoChanged?.Invoke();
            return true;
        }
        return false;
    }    

    private void ConsumeGoodsButtonClick(int number)
    {
        // 재화 상태 확인 절차 진행

        ItemSelect(number);
    }

    #endregion

    #endregion

    #region 가중치 확률 선택

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

    // 확률 변동 없는 캐릭터 뽑기
    private UnitData ReturnData()
    {
        Grade grade = _gradeRandom.GetRandomItem();
        return _data.GetRandomUnitByGrade(grade);
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

    // 확률 변동 있는 캐릭터 뽑기
    private UnitData ReturnDataBySub()
    {
        Grade grade = _gradeRandom.GetRandomItemBySub();
        return _data.GetRandomUnitByGrade(grade);
    }

    #endregion
}