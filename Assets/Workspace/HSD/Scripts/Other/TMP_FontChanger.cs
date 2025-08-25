using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMP_FontChanger : MonoBehaviour
{
    [SerializeField] TMP_FontAsset newFont;

    [ContextMenu("Change Fonts")]
    public void ChangeFonts()
    {
        TMP_Text[] texts = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);

        foreach (TMP_Text text in texts)
        {
            text.font = newFont;
        }
    }
}
