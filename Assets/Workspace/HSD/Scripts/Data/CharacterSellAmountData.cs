using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSellAmountData", menuName = "Data/CharacterSellAmountData")]
public class CharacterSellAmountData : ScriptableObject
{
    private static readonly Dictionary<int, int> _characterSellAmountDic = new Dictionary<int, int>();

    [SerializeField] private CharacterSellAmount[] _characterSellAmounts;

    public int GetSellAmount(int level)
    {        
        if (!_characterSellAmountDic.ContainsKey(level))
        {
            Init();
        }
        
        return _characterSellAmountDic[level];
    }

    private void Init()
    {
        foreach (var item in _characterSellAmounts)
        {
            if (!_characterSellAmountDic.ContainsKey(item.Level))
            {
                _characterSellAmountDic.Add(item.Level, item.SellAmount);
            }
        }
    }
}

[Serializable]
public class CharacterSellAmount
{
    public int Level;
    public int SellAmount;    
}
