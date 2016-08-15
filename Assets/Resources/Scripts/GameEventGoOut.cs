using UnityEngine;
using System.Collections;
using System;

public class GameEventGoOut : GameEvent
{
    public override void Init()
    {
        eventName = "Go Out";
    }

    public override bool ConditionCheck()
    {
        if (GameManager.Instance.GameDate.DayOfWeek <= 5)
        {
            int percent = UnityEngine.Random.Range(1, 100);

            if(percent <= 0.001f)
            {
                return true;
            }
        }

        return false;
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Go Out");
    }

    public override void EventEnded()
    {
        int check = ConversationManager.Instance.GetParameter("Check");
        if (check == 1)
        {
            SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.Movie, 18, false);
            SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.Movie, 19, false);
            SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.Movie, 20, false);

            int weekDif = 6 - GameManager.Instance.GameDate.DayOfWeek;
            Date date = GameManager.Instance.GameDate;
            date.Day += weekDif;
            SchedulingManager.Instance.SetReserveDate(date);
        }

        GameManager.Instance.ScheduleExecute();
    }
}
