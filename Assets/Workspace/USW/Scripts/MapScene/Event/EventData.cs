using UnityEngine;

namespace Map
{
    public enum EventOutcome
    {
        Success,    
        Failure,    
        Declined   
    }

    [CreateAssetMenu(menuName = "Map/Event Data")]
    public class EventData : ScriptableObject
    {
        public string _eventTitle;
        
        [TextArea(3, 10)]
        public string _eventDescription;
        
        [Header("Button Text")]
        public string _yesButtonText = "Yes";   
        public string _nvmButtonText = "Nvm";   
        
        [Header("Energy Cost")]
        public int _energyCost = 1; // 선택지중 Yes 할시 코스트 드는거.
        
        [Header("Success")]
        [TextArea(3, 10)]
        public string _successText;
        public OutGameRewardData _successReward; 
        
        [Header("Failure")]
        [TextArea(3, 10)]
        public string _failureText;
        
        [Header("Declined (Nvm 선택)")]
        [TextArea(3, 10)]
        public string _declinedText;
    }
}