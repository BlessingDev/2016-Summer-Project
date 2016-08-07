using UnityEngine;
using System.Collections;
using System;

public class GameEventInterview : GameEvent
{
    public override void Init()
    {
        eventName = "Interview";
    }

    public override bool ConditionCheck()
    {
        return false;
    }

    public override void ExecuteEvent()
    {
        
    }

    public override void EventEnded()
    {
        GameManager.Instance.SetParameter("InterviewScore",
            ConversationManager.Instance.GetParameter("Score"));
        GameManager.Instance.scheduleButtonType = ScheduleButtonType.Schedule;

        SceneManager.Instance.ChangeScene("GameScene");
    }
}