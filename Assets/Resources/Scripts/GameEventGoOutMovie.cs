using UnityEngine;
using System.Collections;
using System;

public class GameEventGoOutMovie : GameEvent
{
    public override void Init()
    {
        eventName = "Go Out Movie";
    }

    public override bool ConditionCheck()
    {
        if (GameManager.Instance.GameDate.DayOfWeek <= 5)
        {
            float percent = UnityEngine.Random.Range(0f, 100f);

            if(percent <= 0.01f)
            {
                return true;
            }
        }

        return false;
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Go Out Movie");
    }

    public override void EventEnded()
    {
        int check = ConversationManager.Instance.GetParameter("Check");
        if (check == 1)
        {
            SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.Movie, 18, false);
            SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.Movie, 19, false);
            SchedulingManager.Instance.SetReservedScheduleAt(ScheduleType.Movie, 20, false);

            SchedulingManager.Instance.initTime = false;
            int weekDif = 6 - GameManager.Instance.GameDate.DayOfWeek;
            Date date = GameManager.Instance.GameDate;
            date.Day += weekDif;
            SchedulingManager.Instance.SetReserveDate(date);
        }

        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
