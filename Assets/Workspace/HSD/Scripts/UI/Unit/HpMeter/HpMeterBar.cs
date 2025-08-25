using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpMeterBar : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] TMP_Text _hpText;
    private UnitBase[] _units;
    private HpMeterPresenter _hpMeterPresenter;

    public void Init(UnitBase[] units)
    {
        _units = units;
        _hpSlider.maxValue = GetAllMaxHp(units);
        
        if(_hpMeterPresenter != null)
            _hpMeterPresenter.Dispose();

        _hpMeterPresenter = new HpMeterPresenter(units, this);

        Reflash(0);
    }

    private int GetAllMaxHp(UnitBase[] units)
    {
        int max = 0;

        foreach (var unit in units)
        {
            max += unit.StatusController.MaxHealth.Value;
        }

        return max;
    }

    public void Reflash(int amount)
    {
        int currentHp = 0;

        foreach (var unit in _units)
        {
            currentHp += unit.StatusController.CurHp.Value;
        }

        _hpText.text = Utils.ToAbbreviation(currentHp);
        _hpSlider.value = currentHp;
    }
}
