using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListPanel_Agument : ItemListPanel
{
    [SerializeField] ItemSlot_Augment[] _itemSlot_Augments;

    public override void SettingItmes()
    {
        HashSet<AUGData> usedAugments = new HashSet<AUGData>();

        for (int i = 0; i < _itemSlot_Augments.Length; i++)
        {
            int index = i;
            AUGData augmentData;

            // 중복 방지 루프
            do
            {
                augmentData = Manager.Data.AugmentChanceData.GetAugmentData();
            }
            while (usedAugments.Contains(augmentData));

            usedAugments.Add(augmentData);

            int price = Manager.Data.PriceDatas.GetAugumentPrice(augmentData.Grade);

            ItemSlot_Augment itemSlot_Augment = _itemSlot_Augments[index];
            itemSlot_Augment.SetAugmentItem(augmentData);

            _pricePanel.AddListenerButton(
                index,
                () => Buy(itemSlot_Augment, augmentData, price, () => _pricePanel.Close(index)),
                price
            );

            _pricePanel.SetPosition(index, (RectTransform)itemSlot_Augment.transform);
            itemSlot_Augment.ResetSoldOut();
        }
    }

    private void Buy(ItemSlot slot, AUGData augmentData, int price, Action buttonCloseAction)
    {
        if(InGameManager.Instance.SpendGold(price))
        {
            buttonCloseAction?.Invoke();
            slot.SoldOut();
            AugmentManager.Instance.SelectAugment(augmentData);
        }
        else
        {
            Debug.Log("골드 부족");
        }
    }
}
