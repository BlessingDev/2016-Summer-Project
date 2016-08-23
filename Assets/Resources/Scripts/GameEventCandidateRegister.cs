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
            gameDate.Month == 6 &&
            gameDate.Day == 8)
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
        if(ConversationManager.Instance.GetParameter("SchoolNewspaperCheck") == 1)
        {
            GameManager.Instance.SetParameter("SchoolNewspaperCheck", 1);
        }
        if (ConversationManager.Instance.GetParameter("CafeteriaCheck") == 1)
        {
            GameManager.Instance.SetParameter("CafeteriaCheck", 1);
        }
        if (ConversationManager.Instance.GetParameter("CompetitionCheck") == 1)
        {
            GameManager.Instance.SetParameter("CompetitionCheck", 1);
        }
        if (ConversationManager.Instance.GetParameter("LibraryCheck") == 1)
        {
            GameManager.Instance.SetParameter("LibraryCheck", 1);
        }

        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
