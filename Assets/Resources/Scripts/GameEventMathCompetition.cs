using UnityEngine;
using System.Collections;
using System;

public class GameEventMathCompetition : GameEvent
{
    public override void Init()
    {
        eventName = "Math Competition";
    }

    public override bool ConditionCheck()
    {
        if(GameManager.Instance.GameDate.DayOfWeek <= 4)
        {
            float percent = UnityEngine.Random.Range(0f, 100f);

            if (percent <= 0.01f)
            {
                return true;
            }
        }

        
        return false;
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Math Competition");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.MathCompetition, 9, false);
        SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.MathCompetition, 10, false);
        SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.MathCompetition, 11, false);

        Date gameDate = GameManager.Instance.GameDate;
        gameDate.Day += 1;
        SchedulingManager.Instance.SetReserveDate(gameDate);
        GameManager.Instance.latestCompetitionDate = gameDate;
        GameManager.Instance.latestCompetitionName = "수학경시대회";
        GameManager.Instance.GradeCompetition("Math");

        GameManager.Instance.ScheduleExecute();
    }
}
