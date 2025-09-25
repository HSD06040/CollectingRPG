using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogResult
{
    [Header("Result Info")]
    public string resultId;          
    [TextArea(2, 4)]
    public string resultText;        
    
    [Header("Reward (TODO)")]
    public int rewardType;           // TODO: 보상 타입
    public int rewardAmount;         // TODO: 보상 수량
    public string rewardDescription; // TODO: 보상 설명
}

[System.Serializable]
public class DialogChoice
{
    [Header("Choice Info")]
    [TextArea(1, 3)]
    public string choiceText;        
    
    [Header("Results")]
    public List<DialogResult> results = new List<DialogResult>(); 
    
    // 랜덤 결과 선택 
    public DialogResult GetRandomResult()
    {
        if (results.Count == 0) return null;
        if (results.Count == 1) return results[0];
        
        return results[Random.Range(0, results.Count)];
    }
}

[System.Serializable]
public class DialogEvent
{
    [Header("Event Info")]
    public string eventId;          
    [TextArea(3, 6)]
    public string dialogText;       
    
    [Header("Visual")]
    public Sprite eventImage;        
    public AudioClip eventSound;     
    
    [Header("Choices")]
    public List<DialogChoice> choices = new List<DialogChoice>();
}