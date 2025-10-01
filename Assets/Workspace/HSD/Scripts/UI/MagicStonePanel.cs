using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStonePanel : MonoBehaviour
{
    [SerializeField] MagicStoneSlot[] _magicStoneSlots;
    [SerializeField] Transform _dropArea;

    private void Awake()
    {
        foreach (var slot in _magicStoneSlots)
        {
            slot.Init(_dropArea);
        }
    }

    public bool TrySetMagicStone(MagicStoneData magicStoneData)
    {
        foreach (var slot in _magicStoneSlots)
        {
            if (slot.MagicStoneData == null)
            {
                slot.SetMagicStone(magicStoneData);
                return true;
            }
        }

        return false;
    }

    public void ClearAllMagicStones()
    {
        foreach (var slot in _magicStoneSlots)
        {
            slot.ClearMagicStone();
        }
    }
}
