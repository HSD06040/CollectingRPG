using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaResultUISlot : MonoBehaviour
{
    [SerializeField] Image _resultImage;

    public void UpdateUI(Sprite sprite)
    {
        _resultImage.sprite = sprite;
    }
}
