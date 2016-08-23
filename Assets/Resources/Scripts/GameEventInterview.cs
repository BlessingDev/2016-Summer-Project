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
        GameManager.scheduleButtonType = ScheduleButtonType.Schedule;

        Date gameDate = GameManager.Instance.GameDate;
        gameDate.Day += 1;
        GameManager.Instance.GameDate = gameDate;
        SceneManager.Instance.ChangeScene("GameScene");
    }
}