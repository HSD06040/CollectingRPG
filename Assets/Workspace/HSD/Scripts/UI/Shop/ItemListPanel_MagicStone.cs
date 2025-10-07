using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListPanel_MagicStone : ItemListPanel
{
    [SerializeField] ItemSlot_MagicStone[] _itemSlot_MagicStone;

    public override void SettingItmes()
    {
        HashSet<MagicStoneData> usedStones = new HashSet<MagicStoneData>();

        for (int i = 0; i < _itemSlot_MagicStone.Length; i++)
        {
            int index = i;
            MagicStoneData magicStoneData;

            // 중복 방지
            do
            {
                magicStoneData = Manager.Data.MagicStones[
                    UnityEngine.Random.Range(0, Manager.Data.MagicStones.Length)
                ].GetMagicStone();
            }
            while (usedStones.Contains(magicStoneData));

            usedStones.Add(magicStoneData);

            int price = Manager.Data.PriceDatas.GetMagicStonePrice(magicStoneData.Grade);

            _itemSlot_MagicStone[index].SetMagicStoneItem(magicStoneData);

            _pricePanel.AddListenerButton(index,
                () => Buy(magicStoneData, price, () => _pricePanel.Close(index)),
                price);
        }
    }


    private void Buy(MagicStoneData magicStoneData, int price, Action closeAction)
    {
        if (MagicStoneController.Instance.IsFull())
        {
            Debug.Log("마법석칸이 부족합니다.");
            return;
        }

        if (InGameManager.Instance.SpendGold(price))
        {
            closeAction?.Invoke();
            MagicStoneController.Instance.TrySetMagicStone(magicStoneData);
        }
        else
        {
            Debug.Log("골드 부족");
        }
    }
}
