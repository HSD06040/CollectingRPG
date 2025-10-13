using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListPanel_MagicStone : ItemListPanel
{
    [SerializeField] ItemSlot_MagicStone[] _itemSlot_MagicStone;

    private void Awake()
    {
        ToolTipSetting();
    }

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

            ItemSlot_MagicStone itemSlot_MagicStone = _itemSlot_MagicStone[index];
            itemSlot_MagicStone.SetMagicStoneItem(magicStoneData);

            _pricePanel.AddListenerButton(index,
                () => Buy(itemSlot_MagicStone, magicStoneData, price, () => _pricePanel.Close(index)),
                price);

            _pricePanel.SetPosition(index, (RectTransform)itemSlot_MagicStone.transform);
            itemSlot_MagicStone.ResetSoldOut();
        }
    }


    private void Buy(ItemSlot slot, MagicStoneData magicStoneData, int price, Action closeAction)
    {
        if (MagicStoneController.Instance.IsFull())
        {
            UIManager.Instance.MessagePopup.Show("마법석 칸이 꽉찼습니다!");
            return;
        }

        if (InGameManager.Instance.SpendGold(price))
        {
            closeAction?.Invoke();
            slot.SoldOut();
            MagicStoneController.Instance.TrySetMagicStone(magicStoneData);
        }
        else
        {
            Debug.Log("골드 부족");
        }
    }

    private void ToolTipSetting()
    {
        foreach (var slot in _itemSlot_MagicStone)
        {
            slot.ToolTipSet(_shopToolTip);
        }
    }
}
