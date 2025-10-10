using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyActiveSlot_Controller : MonoBehaviour
{
    [SerializeField] SynergyActiveSlot[] _synergyActiveSlots;

    public void Init(Color synergyColor)
    {
        for (int i = 0; i < _synergyActiveSlots.Length; i++)
        {
            _synergyActiveSlots[i].SetColor(synergyColor);
        }
    }

    public void Active(int activeCount)
    {
        for (int i = 0; i < _synergyActiveSlots.Length; i++)
        {
            if (i < activeCount)
            {
                _synergyActiveSlots[i].Active();
            }
            else
            {
                _synergyActiveSlots[i].Deactive();
            }
        }
    }
}
