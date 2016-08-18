using UnityEngine;
using System.Collections;
using System;

public class GameEventVacationCeremony : GameEvent
{
    public override void Init()
    {
        eventName = "Vacation Ceremony";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if((gameDate.Month == 7 && gameDate.Day == 15) &&
            SchedulingManager.Instance.GameTime >= 5 && 
            SchedulingManager.Instance.GameTime <= 6)
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
        SchedulingManager.Instance.initTime = false;
        SchedulingManager.Instance.AddGameTime(1);
        GameManager.Instance.SetVacationSteaker();
        GameManager.Instance.ScheduleExecute();
    }
}
