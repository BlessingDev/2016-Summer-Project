using UnityEngine;
using System.Collections;
using System;

public class GameEventVacationCeremony : GameEvent
{
    public override void Init()
    {
        name = "Vacation Ceremony";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if((gameDate.Month == 7 && gameDate.Day == 15))
        {
            return true;
        }

        return false;
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Vacation Ceremony");
    }

    public override void EventEnded()
    {
        GameManager.Instance.SetVacationSteaker();   
    }
}
