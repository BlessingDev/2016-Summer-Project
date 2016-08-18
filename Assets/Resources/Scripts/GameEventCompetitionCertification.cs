using UnityEngine;
using System.Collections;
using System;

public class GameEventCompetitionCertification : GameEvent
{
    private bool executed = false;

    public override void Init()
    {
        eventName = "Competition Certification";
    }

    public override bool ConditionCheck()
    {
        if(!executed)
        {
            Date compDate = GameManager.Instance.latestCompetitionDate;
            Date gameDate = GameManager.Instance.GameDate;

            compDate.Day += 3;
            if (compDate == gameDate && SchedulingManager.Instance.GameTime >= 17f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Certification");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
