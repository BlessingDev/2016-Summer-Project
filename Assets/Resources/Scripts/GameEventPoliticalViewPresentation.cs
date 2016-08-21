using UnityEngine;
using System.Collections;
using System;

public class GameEventPoliticalViewPresentation : GameEvent {

    static bool executed = false;

    public override void Init()
    {
        eventName = "Political View Presentation";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if (!executed &&
            gameDate.Year == 2 &&
            gameDate.Month == 5 &&
            gameDate.Day == 14 &&
            SchedulingManager.Instance.GameTime >= 17f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ExecuteEvent()
    {
        executed = true;
        ConversationManager.Instance.StartConversationEvent("Political View Presentation");
    }

    public override void EventEnded()
    {
        GameManager.Instance.ScheduleExecute();
    }
}
