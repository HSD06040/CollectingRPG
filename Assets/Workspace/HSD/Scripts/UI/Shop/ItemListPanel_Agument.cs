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

            _itemSlot_Augments[index].SetAugmentItem(augmentData);

            _pricePanel.AddListenerButton(
                index,
                () => Buy(augmentData, price, () => _pricePanel.Close(index)),
                price
            );
        }
    }

    private void Buy(AUGData augmentData, int price, Action buttonCloseAction)
    {
        if(InGameManager.Instance.SpendGold(price))
        {
            buttonCloseAction?.Invoke();
            AugmentManager.Instance.SelectAugment(augmentData);
        }
        else
        {
            Debug.Log("골드 부족");
        }
    }
}
