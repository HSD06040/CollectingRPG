using Firebase.Database;
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
    [SerializeField] private int digits = 2;

    [Header("DB Test")]
    [SerializeField] private UnitData _testData;
    [SerializeField] private Button _testCharacterGachaButton;

    private WeightedRandom<Grade> _gradeCharRandom = new WeightedRandom<Grade>();
    private WeightedRandom<int>[] _gradeCharPieceRandom = new WeightedRandom<int>[4];

    private void Awake()
    {
        Init();
    }

    #region Init

    private void Init()
    {
        // 확률 테이블 초기화
        RandomInit(_prob);

        // 가챠 종류 전환용 버튼 이벤트
        _characterGachaButton.onClick.AddListener(() => SetActivePanel("CharacterGacha"));
        _stoneGachaButton.onClick.AddListener(() => SetActivePanel("MagicStoneGacha"));

        // 캐릭터 가챠에 대한 버튼 이벤트
        _dailyCharacterGachaButton.onClick.AddListener(AdButtonClick);
        _oneCharacterGachaButton.onClick.AddListener(OneButtonClick);
        _tenCharacterGachaButton.onClick.AddListener(() => ConsumeGoodsButtonClick(10));

        // 테스트 기능
        _testCharacterGachaButton.onClick.AddListener(TestItemSelect);
    }

    /// <summary>
    /// 확률표 데이터(SO)를 바탕으로 확률 초기화.
    /// * 유의사항 - 확률표에서 소수점의 길이만큼 digits를 늘려줄 것.
    /// ex) 확률표상 소수점 네 자리(70.4356) -> digits : 최소 4 이상의 수 입력
    /// </summary>
    /// <param name="probability"></param>
    private void RandomInit(ItemProbabilitySO probability)
    {
        for(int i  = 0; i < _gradeCharPieceRandom.Length; i++)
        {
            _gradeCharPieceRandom[i] = new WeightedRandom<int>();
        }

        for (int i = 0; i < probability.ItemsProbability.Count; i++)
        {
            Grade grade = probability.ItemsProbability[i].ItemGrade;
            int value = (int)(probability.ItemsProbability[i].Probability * Math.Pow(10, digits));
            _gradeCharRandom.Add(grade, value);

            for(int j = 0; j < probability.ItemsProbability[i].Pieces.Count; j++)
            {
                int pieceValue = (int)(probability.ItemsProbability[i].Pieces[j].PieceProbability * Math.Pow(10, digits));
                _gradeCharPieceRandom[i].Add(probability.ItemsProbability[i].Pieces[j].PieceNum, pieceValue);
            }
        }
    }

    #endregion

    #region Button Click Event

    #region GachaListButton

    /// <summary>
    /// 선택한 가챠 종류의 패널을 표시.
    /// </summary>
    /// <param name="activePanel"></param>
    private void SetActivePanel(string activePanel)
    {
        _characterGacha.SetActive(activePanel.Equals(_characterGacha.name));
        _stoneGacha.SetActive(activePanel.Equals(_stoneGacha.name));
    }

    #endregion

    #region CharacterGachaButton

    #region AdButton

    /// <summary>
    /// 광고 버튼 클릭 이벤트
    /// </summary>
    private void AdButtonClick()
    {
        // 일일 광고 가챠가 가능할 경우 실행
        if (DailyAdGacha()) return;

        // 광고 가챠가 불가능할 경우 경고 팝업
        if (PopupManager.Instance != null)
        {
            PopupManager.instance.ShowPopup("일일 광고 가챠를 전부 사용하였습니다.");
        }
    }

    /// <summary>
    /// 광고 가챠의 가능 여부를 판별하고 가능할 시 광고 시청 후 가챠를 진행
    /// </summary>
    /// <returns></returns>
    private bool DailyAdGacha()
    {
        // 광고 가챠가 가능할 때
        if (TimeManager.Instance.CanObtainAdGachaReward())
        {
            // 광고 가챠 쿨타임 업데이트
            TimeManager.Instance.UpdateAdGachaResetTimeInfo();
            // 1회 뽑기 진행
            ItemSelect(1);
            TimeManager.Instance.OnDailyGachaInfoChanged?.Invoke();
            return true;
        }

        return false;
    }

    #endregion

    #region DailyButton

    /// <summary>
    /// 1회 뽑기 버튼 클릭 이벤트
    /// </summary>
    private void OneButtonClick()
    {
        // 일일 무료 뽑기가 가능할 때 해당 뽑기 우선 진행
        if (DailyFreeGacha()) return;

        ConsumeGoodsButtonClick(1);
    }

    /// <summary>
    /// 일일 뽑기의 가능 여부를 판별하고, 가능할 시 횟수를 소모하고 진행
    /// </summary>
    /// <returns></returns>
    private bool DailyFreeGacha()
    {
        // 일일 무료 뽑기가 가능할 때
        if (TimeManager.Instance.CanObtainedFreeGachaReward())
        {
            // 1회 뽑기 진행
            ItemSelect(1);
            // 일일 무료 뽑기 쿨타임 업데이트
            TimeManager.Instance.UpdateDailyFreeGachaResetTimeInfo();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 재화를 소모하고 뽑기를 진행 - 이후 소모하는 재료 종류에 대한 확장성 고려 필요 (input으로 넣기?)
    /// </summary>
    /// <param name="number"></param>
    private async void ConsumeGoodsButtonClick(int number)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        var diaRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("Diamond");

        DataSnapshot snapshot = await diaRef.GetValueAsync();

        int currentDiamond = 0;

        if (snapshot.Exists && snapshot.Value != null)
        {
            currentDiamond = Convert.ToInt32(snapshot.Value);
        }
        Debug.Log(currentDiamond);

        if (currentDiamond >= 300 * number)
        {
            ItemSelect(number);
            await DBManager.Instance.SubtractDiamondAsync(300  * number);
        }
        else
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.instance.ShowPopup("다이아몬드가 부족합니다.");
            }
        }
    }

    #endregion

    #endregion

    #region DB Test

    /// <summary>
    /// 데이터베이스 연동 테스트용 기능. 1회 뽑기와 동일 로직
    /// </summary>
    private async void TestItemSelect()
    {
        UnitData data = _testData;

        // 조각 등장 확률도 나중에 가중치로 전환되면 가중치로 적용 필요
        int pieceNum = UnityEngine.Random.Range(1, 11);

        _testData.UpgradeData.AddPiece(pieceNum);

        _resultUI.HeroGachaUpdate(data, 0, pieceNum.ToString());

        await DBManager.Instance.charDB.SaveCharacterUpgradeData(_testData);

        _resultUI.gameObject.SetActive(true);
    }

    #endregion

    #endregion

    #region 가중치 확률 선택

    // 확률 변동이 없는 가중치 확률
    private async void ItemSelect(int number)
    {
        if (_gradeCharRandom.GetList() == null) RandomInit(_prob);        

        for (int i = 0; i < number; i++)
        {
            UnitData data = ReturnData();

            int pieceNum = ReturnPieceByGrade(data);

            data.UpgradeData.AddPiece(pieceNum);

            _resultUI.HeroGachaUpdate(data, i, pieceNum.ToString());

            await DBManager.Instance.charDB.SaveCharacterUpgradeData(data);
        }

        _resultUI.gameObject.SetActive(true);
    }

    // 확률 변동 없는 캐릭터 뽑기
    private UnitData ReturnData()
    {
        Grade grade = _gradeCharRandom.GetRandomItem();
        return _data.GetRandomUnitByGrade(grade);
    }

    private int ReturnPieceByGrade(UnitData data)
    {
        Grade grade = data.Grade;
        int piece = _gradeCharPieceRandom[(int)grade].GetRandomItem();
        return piece;
    }


    // 천장이 있는 가중치 확률
    private void ItemSelectBySub(int number)
    {
        if (_gradeCharRandom.GetList() == null) RandomInit(_prob);

        for (int i = 0; i < number; i++)
        {
            ReturnDataBySub();
        }
    }

    // 확률 변동 있는 캐릭터 뽑기
    private UnitData ReturnDataBySub()
    {
        Grade grade = _gradeCharRandom.GetRandomItemBySub();
        return _data.GetRandomUnitByGrade(grade);
    }

    #endregion
}