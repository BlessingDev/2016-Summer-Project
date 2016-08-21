using UnityEngine;
using System.Collections;
using System;

public class GameEventCandidateRegister : GameEvent {

    static bool executed = false;

    public override void Init()
    {
        eventName = "Candidate Register";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if (!executed &&
            gameDate.Year == 2 &&
            gameDate.Month == 5 &&
            gameDate.Day == 4)
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

        ConversationManager.Instance.StartConversationEvent("Candidate Register");
    }

    public override void EventEnded()
    {
        //나중에 채워야 할 부분
    }
}
