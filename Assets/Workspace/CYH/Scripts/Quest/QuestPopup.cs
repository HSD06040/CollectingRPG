using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestPopup : MonoBehaviour
{
    [SerializeField] private QuestManager _questManager;

    [Header("List")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private GameObject _QuestPrefab;

    [Header("UI")]
    [SerializeField] private TMP_Text _remainTime;
    [SerializeField] private Image _progressBar;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (var quest in _questManager._quests)
        {
            if (quest is IQuestView view)
            {
                GameObject questItem = Instantiate(_QuestPrefab, _content);
                questItem.GetComponent<QuestItem>().Init(view);
            }
        }
    }
}
