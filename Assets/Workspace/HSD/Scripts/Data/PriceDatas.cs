using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PriceData", menuName = "Data/PriceData")]
public class PriceDatas : ScriptableObject
{
    [SerializeField] PriceData[] _magicStonPriceDatas;
    [SerializeField] PriceData[] _augumentPriceDatas;
    private static readonly Dictionary<SubGrade, int> _magicStonPriceDic = new Dictionary<SubGrade, int>();
    private static readonly Dictionary<SubGrade, int> _augumentPriceDic = new Dictionary<SubGrade, int>();

    public int GetMagicStonePrice(SubGrade subGrade)
    {
        if (!_magicStonPriceDic.ContainsKey(subGrade))
            Init();

        return _magicStonPriceDic[subGrade];
    }

    public int GetAugumentPrice(SubGrade subGrade)
    {
        if (!_augumentPriceDic.ContainsKey(subGrade))
            Init();

        return _augumentPriceDic[subGrade];
    }

    private void Init()
    {
        foreach (var item in _magicStonPriceDatas)
        {
            if (!_magicStonPriceDic.ContainsKey(item.SubGrade))
                _magicStonPriceDic.Add(item.SubGrade, item.Price);
        }

        foreach (var augmentPriceData in _augumentPriceDatas)
        {
            if (!_augumentPriceDic.ContainsKey(augmentPriceData.SubGrade))
                _augumentPriceDic.Add(augmentPriceData.SubGrade, augmentPriceData.Price);
        }
    }
}

[System.Serializable]
public class PriceData
{
    public SubGrade SubGrade;
    public int Price;
}
