using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemSlot : MonoBehaviour
{
    [SerializeField] TMP_Text _itemNameText;
    [SerializeField] Image _itemIcon;
    [SerializeField] Image _soldOutImage;
    [SerializeField] GameObject[] _gradeImages;
    [SerializeField] protected ShopToolTip _shopToolTip;

    public void SetItem(string itemName, Sprite itemIcon)
    {
        _itemNameText.text = itemName;
        _itemIcon.sprite = itemIcon;
    }

    public void SoldOut()
    {
        _soldOutImage.gameObject.SetActive(true);
    }

    public void ResetSoldOut()
    {
        _soldOutImage.gameObject.SetActive(false);
    }

    protected void SetGradeImage(SubGrade grade)
    {
        for (int i = 0; i < _gradeImages.Length; i++)
        {
            if(i == (int)grade)
            {
                _gradeImages[i].SetActive(true);
            }
            else
            {
                _gradeImages[i].SetActive(false);
            }
        }
    }

    public void ToolTipSet(ShopToolTip shopToolTip)
    {
        _shopToolTip = shopToolTip;
    }
}
