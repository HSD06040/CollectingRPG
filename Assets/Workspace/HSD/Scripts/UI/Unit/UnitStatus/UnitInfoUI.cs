using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoUI : MonoBehaviour
{
    [SerializeField] RectTransform _unitPanel;
    [SerializeField] CanvasGroup _unitInfoPanelGroup;
    [SerializeField] CanvasGroup _unitPanelGroup;
    [SerializeField] float _fadeDuration = .5f;
    [SerializeField] float _moveDuration = .5f;
    private Vector2 _originPos;
    private Vector2 _downPos;

    [Header("UI")]
    [SerializeField] Image _unitIcon;
    [SerializeField] TMP_Text _unitNameText;
    [SerializeField] Image _synergyImage;
    [SerializeField] Image _classImage;
    [SerializeField] GameObject[] _stars;

    [Header("HP_MP")]
    [SerializeField] Slider _hpSlider;
    [SerializeField] TMP_Text _hpText;
    [SerializeField] Slider _mpSlider;
    [SerializeField] TMP_Text _mpText;

    [Header("Other UI")]
    [SerializeField] UnitSkillUI _unitSkillUI;
    [SerializeField] UnitStatusUI _unitStatusUI;
    [SerializeField] UnitSellOrAutoSelectionUI _unitSellOrAutoSelectionUI;

    private void Start()
    {
        _originPos = _unitPanel.anchoredPosition;
        _downPos = _unitPanel.anchoredPosition + new Vector2(0, -300);
        _unitPanel.anchoredPosition = _downPos;

        _unitPanelGroup.Reset();
        _unitInfoPanelGroup.Reset();
    }

    public void Setup(UnitStatus status, bool isUI, bool isSell, bool isEnemy)
    {
        if (_unitPanelGroup.alpha == 1f)
            return;

        _unitPanelGroup.FadeIn(_fadeDuration).Forget();
        _unitPanel.DOKill(false);

        ShowAnimation().Forget();

        _unitNameText.text = status.Data.Name;
        _unitIcon.sprite = status.Data.Icon;

        int count = 0;
        for (int i = 0; i < status.Level + 1; i++)
        {
            count++;
            _stars[i].gameObject.SetActive(true);
        }

        for (int i = count; i < _stars.Length; i++)
        {
            _stars[i].gameObject.SetActive(false);
        }

        //if(status.Data.UpgradeData != null)
        //    _levelText.text = status.Data.UpgradeData.CurrentUpgradeData.UpgradeLevel.ToString();
        //else
        //    _levelText.text = "0";

        //_powerText.text = status.CombatPower.ToString();

        // isEnemy로 분리

        _synergyImage.sprite = Manager.Data.SynergyDB.GetSynergy((int)status.Data.Synergy).ActiveIcon;
        _classImage.sprite = Manager.Data.SynergyDB.GetSynergy((int)status.Data.ClassSynergy).ActiveIcon;

        UnitStats stat = status.GetCurrentStat();

        _hpSlider.maxValue = stat.MaxHealth;
        _hpSlider.value = stat.MaxHealth;
        _hpText.text = $"{stat.MaxHealth}/{stat.MaxHealth}";

        _mpSlider.maxValue = stat.MaxMana;
        _mpSlider.value = stat.MaxMana;
        _mpText.text = $"{stat.MaxMana}/{stat.MaxMana}";

        _unitSkillUI.Setup(status);
        _unitStatusUI.Setup(stat);

        if (isSell)
        {
            _unitSellOrAutoSelectionUI.Setup(status, isUI, Close);
        }
        else
        {
            _unitSellOrAutoSelectionUI.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        _unitPanelGroup.FadeOut(_fadeDuration).Forget();
    }

    private async UniTask ShowAnimation()
    {
        _unitInfoPanelGroup.FadeIn(_moveDuration).Forget();
        _unitPanel.anchoredPosition = _downPos;

        await _unitPanel.DOAnchorPos(_originPos, _moveDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
}
