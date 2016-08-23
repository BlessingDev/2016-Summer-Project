using UnityEngine;
using System.Collections;
using System;

public class GameEventUniversityResult : GameEvent {

    private static bool executed = false;

    public override void Init()
    {
        eventName = "University Result";
    }

    public override bool ConditionCheck()
    {
        if(!executed &&
            GameManager.Instance.GameDate == new Date(3, 11, 1))
        {
            return true;
        }

        return false;
    }

    public override void ExecuteEvent()
    {
        executed = true;
        ConversationManager.Instance.StartConversationEvent("University Result");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
