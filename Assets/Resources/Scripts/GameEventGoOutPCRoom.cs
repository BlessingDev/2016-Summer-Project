using UnityEngine;
using System.Collections;
using System;

public class GameEventGoOutPCRoom : GameEvent {

    public override void Init()
    {
        eventName = "Go Out PC";
    }

    public override bool ConditionCheck()
    {
        int weekDay = GameManager.Instance.GameDate.DayOfWeek;
        float time = SchedulingManager.Instance.GameTime;

        if (weekDay >= 1 && weekDay <= 5 &&
            time >= 9f && time <= 17f)
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
        ConversationManager.Instance.StartConversationEvent("Go Out PC");
    }

    public override void EventEnded()
    {
        int check = ConversationManager.Instance.GetParameter("Check");
        if (check == 1)
        {
            SchedulingManager.Instance.SetSchedule(18, ScheduleType.PCRoom);
            SchedulingManager.Instance.SetSchedule(19, ScheduleType.PCRoom);

            SchedulingManager.Instance.clearSchedule = true;
            GameManager.isLockSchedule = true;
        }

        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
