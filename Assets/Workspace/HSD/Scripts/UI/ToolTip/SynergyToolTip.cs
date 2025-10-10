using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SynergyToolTip : ToolTip
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _synergyName;
    [SerializeField] TMP_Text _description;
    [SerializeField] TMP_Text _effectDescription;
    private const string WHITE = "#FFFFFF";

    public void Show(PointerEventData data, SynergyData synergyData)
    {
        gameObject.SetActive(true);
        Setup(synergyData);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Setup(SynergyData synergyData)
    {
        _icon.sprite = synergyData.ActiveIcon;
        _synergyName.text = synergyData.SynergyName;
        _description.text = synergyData.Description;

        _effectDescription.color = Color.gray;
        _effectDescription.text = GetEffectDescription(synergyData);

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
    }

    private string GetEffectDescription(SynergyData synergyData)
    {
        Utils.ClearStringBuilder();

        for (int i = 0; i < synergyData.SynergyLevelData.Length; i++)
        {
            SynergyLevelData levelData = synergyData.SynergyLevelData[i];

            if(synergyData.CurrentUpgradeIdx == i)
                Utils.AppendLine(GetWhiteColorString($"({levelData.SynergyNeedCount}) {levelData.Effects[0].Description}"));
            else
                Utils.AppendLine($"({levelData.SynergyNeedCount}) {levelData.Effects[0].Description}");
        }

        return Utils.GetString();
    }   

    private string GetWhiteColorString(string str)
    {
        return $"<color={WHITE}>{str}</color>";
    }
}
