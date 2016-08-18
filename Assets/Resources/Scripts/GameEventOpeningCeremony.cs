using UnityEngine;
using System.Collections;
using System;

public class GameEventOpeningCeremony : GameEvent
{
    public override void Init()
    {
        eventName = "Opening Ceremony";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if((gameDate.Month == 8 && gameDate.Day == 17) &&
            SchedulingManager.Instance.GameTime >= 5 &&
            SchedulingManager.Instance.GameTime <= 6)
        {
            return true;
        }

        return false;
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Opening Ceremony");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        SchedulingManager.Instance.AddGameTime(1);
        GameManager.Instance.SetOpeningSteaker();
        SchedulingManager.Instance.stopReserve = true;
        SchedulingManager.Instance.clearSchedule = true;

        Date gameDate = GameManager.Instance.GameDate;
        if (gameDate.Year == 1 && gameDate.Month == 8)
            GameManager.Instance.SetFirstGradeSecondSubjects();
        else if (gameDate.Year == 2 && gameDate.Month == 3)
            GameManager.Instance.SetSecondGradeFirstSubjects();

        GameManager.Instance.ScheduleExecute();
    }
}
