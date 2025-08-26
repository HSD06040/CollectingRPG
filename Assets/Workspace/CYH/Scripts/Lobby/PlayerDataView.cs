using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDataView : MonoBehaviour
{
    // 찾은 UI들 캐싱
    private Dictionary<string, List<TMP_Text>> cachedTexts = new Dictionary<string, List<TMP_Text>>();
    //[SerializeField] private TMP_Text _playerNameText;
    //[SerializeField] private TMP_Text _goldText;
    //[SerializeField] private TMP_Text _diamondText;


    private void OnEnable()
    {
        if (cachedTexts.Count > 0)
        {
            cachedTexts.Clear();
        } 
    }

    public void UpdateUI(PlayerData data)
    {
        var dataType = typeof(PlayerData);
        var fields = dataType.GetFields();

        foreach (var field in fields)
        {
            var value = field.GetValue(data);
            string textValue = FormatValue(field.Name, value);
            UpdateTextsByName(field.Name + "Text", textValue);
        }

        //  _playerNameText.text = data.PlayerName;
        //  _goldText.text = $"{data.Gold}";
        //  _diamondText.text = $"{data.Diamond}";
    }
    
    // 이름으로 텍스트 업데이트
    private void UpdateTextsByName(string objectName, string value)
    {
        // 캐시에서 먼저 확인
        if (!cachedTexts.ContainsKey(objectName))
        {
            var foundTexts = new List<TMP_Text>();
            var allTexts = FindObjectsOfType<TMP_Text>(true); 

            foreach (var text in allTexts)
            {
                if (text.name.Equals(objectName, StringComparison.OrdinalIgnoreCase))
                {
                    foundTexts.Add(text);
                }
            }

            cachedTexts[objectName] = foundTexts;
        }

        // 찾은 모든 텍스트 업데이트
        foreach (var text in cachedTexts[objectName])
        {
            if (text != null)
            {
                text.text = value;
            }
        }
    }

    // 값 포맷팅
    private string FormatValue(string fieldName, object value)
    {
        return fieldName switch
        {
            "Gold" => $"{value:N0}", 
            "Diamond" => $"{value:N0}", 
            _ => value?.ToString() ?? ""
        };
    }
}