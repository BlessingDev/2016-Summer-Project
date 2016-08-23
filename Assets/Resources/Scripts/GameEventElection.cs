using UnityEngine;
using System.Collections;

public class GameEventElection : GameEvent {

    static bool executed = false;

    public override void Init()
    {
        eventName = "Election";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if (!executed &&
            gameDate.Year == 2 &&
            gameDate.Month == 6 &&
            gameDate.Day == 21 &&
            SchedulingManager.Instance.GameTime >= 17f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ExecuteEvent()
    {
        executed = true;
        ConversationManager.Instance.StartConversationEvent("Election");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        SchedulingManager.Instance.AddGameTime(1);
        GameManager.Instance.ScheduleExecute();
    }
}
