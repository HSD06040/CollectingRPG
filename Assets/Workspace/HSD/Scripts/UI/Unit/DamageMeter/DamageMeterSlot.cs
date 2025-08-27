using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageMeterSlot : MonoBehaviour, IDamageMeterView
{
    [SerializeField] Image _icon;
    [SerializeField] Slider _damageSlider;
    [SerializeField] TMP_Text _numberText;
    [SerializeField] TMP_Text _damageText;
    [SerializeField] int _totalDamage;
    public int TotalDamage => _totalDamage;

    private DamageMeterPresenter _presenter;

    public static event Action OnDamaged;

    public void Init(UnitStatusController status)
    {        
        if (_presenter != null)
            _presenter.Dispose();

        _presenter = new DamageMeterPresenter(this, status);
    }

    public void SetDamage(int damage)
    {
        _totalDamage = damage;
        _damageText.text = Utils.ToAbbreviation(damage);
        _damageSlider.value = damage;
        OnDamaged?.Invoke();
    }

    public void SetIcon(Sprite icon)
    {
        _icon.sprite = icon;
    }

    public void SetSliderMaxValue(int _maxDamage)
    {
        _damageSlider.maxValue = _maxDamage;        
    }

    public void SetNumber(int num)
    {
        _numberText.text = num.ToString();
    }
    public void RefreshValue()
    {
        _damageSlider.value = _totalDamage;
    }
}
