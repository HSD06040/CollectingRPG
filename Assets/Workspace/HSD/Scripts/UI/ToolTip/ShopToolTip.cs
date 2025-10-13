using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopToolTip : ToolTip
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _description;

    private void Show(Sprite icon, string nameText, string description, Vector2 pos)
    {
        _icon.sprite = icon;
        _nameText.text = nameText;
        _description.text = description;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Show(MagicStoneData magicStoneData, Vector2 pos)
    {
        Show(magicStoneData.Icon, magicStoneData.Name, magicStoneData.Description, pos);
    }

    public void Show(AUGData augmentData, Vector2 pos)
    {
        Show(augmentData.Icon, augmentData.Name, augmentData.Description, pos);
    }   
}
