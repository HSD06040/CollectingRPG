using UnityEngine;
using UnityEngine.UI;

public class LobbyClassSynergySlot : MonoBehaviour
{
    [SerializeField] private Image _icon;

    public int ActiveCount { get; private set; }
    public int UpgradeCount => ActiveCount / 2;

    public void SetSynergy(ClassType classSynergy)
    {
        _icon.sprite = SynergyController.SynergyDB.GetSynergy((int)classSynergy).Icon;
    }

    public void SetActiveCount(int count)
    {
        ActiveCount = count;

        gameObject.SetActive(count >= 2);
    }
}