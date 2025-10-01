using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaUIController : MonoBehaviour
{
    [Header("AdButtonUI")]
    [SerializeField] private Image[] _adImages;
    [SerializeField] private GameObject _adCooltimeImage;
    [SerializeField] private TMP_Text _adCooltimeText;

    private Coroutine _adCooltimeTimer;

    [Header("OneButtonUI")]
    [SerializeField] private TMP_Text _oneText;
    [SerializeField] private GameObject _freeGacha;
    [SerializeField] private GameObject _consumeGacha;
    [SerializeField] private GameObject _dailyCooltimeImage;
    [SerializeField] private TMP_Text _dailyCooltimeText;

    private Coroutine _dailyCooltimeTimer;

    private void Start()
    {
        UpdateUI();
    }

    #region Event

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDailyGachaInfoChanged += UpdateUI;
        StartAdTimer();
        StartDailyTimer();
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnDailyGachaInfoChanged -= UpdateUI;
        StopAdTimer();
        StopDailyTimer();
    }

    #endregion

    #region AdButtonTimer

    private void StartAdTimer()
    {
        if (_adCooltimeTimer != null)
        {
            StopCoroutine(_adCooltimeTimer);
        }
        _adCooltimeTimer = StartCoroutine(AdCooltimeCoroutine());
    }

    private void StopAdTimer()
    {
        if (_adCooltimeTimer != null)
        {
            StopCoroutine(_adCooltimeTimer);
            _adCooltimeTimer = null;
        }
    }

    #endregion

    #region DailyButtonTimer

    private void StartDailyTimer()
    {
        if (_dailyCooltimeTimer != null)
        {
            StopCoroutine(_dailyCooltimeTimer);
        }
        _dailyCooltimeTimer = StartCoroutine(DailyCooltimeCoroutine());
    }

    private void StopDailyTimer()
    {
        if (_dailyCooltimeTimer != null)
        {
            StopCoroutine(_dailyCooltimeTimer);
            _dailyCooltimeTimer = null;
        }
    }

    #endregion

    #region UIUpdate

    private void UpdateUI()
    {
        UpdateAdButton();
        UpdateOneButton();
    }

    private void UpdateAdButton()
    {
        if (TimeManager.Instance != null)
        {
            switch (TimeManager.Instance.DailyCharAdGachaRewardInfo.state)
            {
                case 2:
                    _adImages[0].color = Color.white;
                    _adImages[1].color = Color.white;
                    _adCooltimeImage.gameObject.SetActive(false);
                    break;

                case 1:
                    _adImages[0].color = Color.grey;
                    _adImages[1].color = Color.white;
                    _adCooltimeImage.gameObject.SetActive(true);
                    break;
                case 0:
                    _adImages[0].color = Color.grey;
                    _adImages[1].color = Color.grey;
                    _adCooltimeImage.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateOneButton()
    {
        if (TimeManager.Instance != null)
        {
            if (TimeManager.Instance.DailyCharFreeGachaRewardInfo.state == 1)
            {
                _freeGacha.SetActive(true);
                _consumeGacha.SetActive(false);
                _dailyCooltimeImage.gameObject.SetActive(false);
                _oneText.text = "일일 모집";
            }
            else
            {
                _freeGacha.SetActive(false);
                _consumeGacha.SetActive(true);
                _dailyCooltimeImage.gameObject.SetActive(true);
                _oneText.text = "1회 모집";
            }
        }
    }

    #endregion

    private IEnumerator AdCooltimeCoroutine()
    {
        while (true)
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.CanObtainAdGachaReward(GachaType.Char);

                DateTime lastTime = TimeManager.Instance.DailyCharAdGachaRewardInfo.GetDateTime();
                DateTime now = DateTime.Now;

                TimeSpan cooltime = lastTime.AddHours(12) - now;

                _adCooltimeText.text = $"다음 초기화 : {cooltime.Hours}시간 {cooltime.Minutes}분";
            }

            UpdateAdButton();

            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator DailyCooltimeCoroutine()
    {
        while (true)
        {
            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.CanObtainedFreeGachaReward(GachaType.Char);

                DateTime nextdate = TimeManager.Instance.DailyCharFreeGachaRewardInfo.GetDateTime();
                DateTime now = DateTime.Now;

                TimeSpan cooltime = nextdate - now;
                _dailyCooltimeText.text = $"다음 초기화 : {cooltime.Hours}시간 {cooltime.Minutes}분";
            }

            UpdateOneButton();

            yield return new WaitForSeconds(1);
        }
    }
}