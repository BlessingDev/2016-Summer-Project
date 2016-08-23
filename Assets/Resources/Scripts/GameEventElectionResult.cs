using UnityEngine;
using System.Collections;
using System;

public class GameEventElectionResult : GameEvent {

    private static bool executed = false;

    public override void Init()
    {
        eventName = "Election Result";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if(gameDate == new Date(2, 6, 9) && !executed)
        {
            return true;
        }
        return false;
    }

    public override void ExecuteEvent()
    {
        executed = true;
        ConversationManager.Instance.StartConversationEvent("Election Result");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
