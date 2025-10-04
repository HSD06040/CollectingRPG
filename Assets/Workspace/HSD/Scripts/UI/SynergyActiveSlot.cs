using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyActiveSlot : MonoBehaviour
{
    [SerializeField] GameObject _activePanel;
    
    public void Active()
    {
        _activePanel.SetActive(true);
    }

    public void Deactive()
    {
        _activePanel.SetActive(false);
    }
}
