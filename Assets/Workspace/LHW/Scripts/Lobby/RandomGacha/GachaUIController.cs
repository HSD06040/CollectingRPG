using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaUIController : MonoBehaviour
{
    [SerializeField] private Image[] _adImages;
    [SerializeField] private TMP_Text _oneText;
    [SerializeField] private GameObject _freeGacha;
    [SerializeField] private GameObject _consumeGacha;

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        if (TimeManager.Instance != null)
            TimeManager.Instance.OnDailyGachaInfoChanged += UpdateUI;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnDailyGachaInfoChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        UpdateAdButton();
        UpdateOneBUtton();
    }

    private void UpdateAdButton()
    {
        switch(TimeManager.Instance.DailyAdGachaRewardInfo.state)
        {
            case 2:
                _adImages[0].color = Color.white;
                _adImages[1].color = Color.white;
                break;

           case 1:
                _adImages[0].color = Color.grey;
                _adImages[1].color = Color.white;
                break;
            case 0:
                _adImages[0].color = Color.grey;
                _adImages[1].color = Color.grey;
                break;
        }
    }

    private void UpdateOneBUtton()
    {
        if (TimeManager.Instance.DailyFreeGachaRewardInfo.state == 1)
        {
            _freeGacha.SetActive(true);
            _consumeGacha.SetActive(false);
            _oneText.text = "일일 모집";
        }
        else
        {
            _freeGacha.SetActive(false);
            _consumeGacha.SetActive(true);
            _oneText.text = "1회 모집";
        }
    }
}
