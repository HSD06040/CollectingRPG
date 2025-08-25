using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergySlot : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Image _upgradeColorImage;
    [SerializeField] TMP_Text _synergyName;
    [SerializeField] TMP_Text _synergyCount;
    [SerializeField] TMP_Text _synergyActive;

    private SynergyData _synergyData;
    private const string GRAY = "#808080";
    private const string WHITE = "#FFFFFF";
    private int[] _synergyCountArray;    

    public int ActiveCount;

    public void Init(SynergyData data, int activeCount)
    {        
        _synergyData = data;
        _icon.sprite = data.Icon;
        _synergyName.text = data.SynergyName;
        SetSynergyCount();
        UpdateUI(activeCount);
    }

    public void UpdateUI(int activeCount)
    {
        _synergyActive.color = Color.gray;
        ActiveCount = activeCount;
        _synergyActive.text = GetUpgradeCountString();
        _synergyCount.text = activeCount.ToString();
        _upgradeColorImage.color = _synergyData.GetSynergyColor();
    }   
    
    private string GetUpgradeCountString()
    {        
        for (int i = 0; i < _synergyCountArray.Length; i++)
        {
            if(_synergyData.CurrentUpgradeIdx == i)
            {
                if(i == 0)                
                    Utils.AppendString($"{GetWhiteColorString(_synergyCountArray[i].ToString())}");                
                else
                    Utils.AppendString($"/{GetWhiteColorString(_synergyCountArray[i].ToString())}");
            }
            else
            {
                if(i == 0)
                    Utils.AppendString($"{_synergyCountArray[i]}");
                else                
                    Utils.AppendString($"/{_synergyCountArray[i]}");
            }
        }

        return Utils.GetString();
    }

    private void SetSynergyCount()
    {
        _synergyCountArray = new int[_synergyData.SynergyLevelData.Length];

        for (int i = 0; i < _synergyData.SynergyLevelData.Length; i++)
        {
            _synergyCountArray[i] = _synergyData.SynergyLevelData[i].SynergyNeedCount;
        }
    }

    private string GetWhiteColorString(string str)
    {
        return $"<color={WHITE}>{str}</color>";
    }
}