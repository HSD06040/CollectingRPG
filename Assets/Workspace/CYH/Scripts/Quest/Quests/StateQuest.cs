using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State Quest", menuName = "Quests/State Quest")]
public class StateQuest : QuestDataSO<bool>
{
    public override void ClearCondition(bool state)
    {
        if (state)
        {
            Clear();
        }
    }
}
