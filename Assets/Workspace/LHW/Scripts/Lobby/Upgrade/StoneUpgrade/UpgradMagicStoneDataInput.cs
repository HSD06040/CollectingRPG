using UnityEngine;

public class UpgradMagicStoneDataInput : MonoBehaviour
{
    private MagicStoneUpgradeUnit[] units;
    [SerializeField] MagicStone[] datas => Manager.Data.MagicStones;

    private void Awake() => Init();

    private void Init()
    {        
        units = GetComponentsInChildren<MagicStoneUpgradeUnit>();
        for (int i = 0; i < datas.Length; i++)
        {
            units[i].InitMagicStoneStatus(datas[i]);
        }
    }
}