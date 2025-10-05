using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyActiveSlot : MonoBehaviour
{
    [SerializeField] Image _activePanel;
    
    public void SetColor(Color synergyColor)
    {
        _activePanel.color = synergyColor;
    }

    public void Active()
    {
        _activePanel.gameObject.SetActive(true);
    }

    public void Deactive()
    {
        _activePanel.gameObject.SetActive(false);
    }
}
