using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentPanel : MonoBehaviour
{
    [SerializeField] AugmentSlot[] _augmentSlots;

    public void Setup()
    {
        List<AUGData> augDatas = new List<AUGData>();

        for (int i = 0; i < 3; i++)
        {
            AUGData augmentData;

            do
            {
                augmentData = Manager.Data.AugmentChanceData.GetAugmentData();
            }
            while (augDatas.Contains(augmentData));

            augDatas.Add(augmentData);
        }

        for (int i = 0; i < _augmentSlots.Length; i++)
        {
            _augmentSlots[i].Setup(augDatas[i]);
        }
    }
}
