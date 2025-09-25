using UnityEngine;

[CreateAssetMenu(fileName = "New Stage Dialog Event", menuName = "Stage/Dialog Event")]
public class StageDialogEventSO : ScriptableObject
{
    [Header("Dialog Event")]
    public DialogEvent dialogEvent;
    
    // 편의 프로퍼티들
    public string EventId => dialogEvent.eventId;
    public string DialogText => dialogEvent.dialogText;
    public Sprite EventImage => dialogEvent.eventImage;
    public AudioClip EventSound => dialogEvent.eventSound;
}