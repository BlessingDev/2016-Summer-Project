using UnityEngine;
using System.Collections;
using System;

public class GameEventCourseSelect : GameEvent
{
    static bool executed = false;

    public override void Init()
    {
        eventName = "Course Select";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if(gameDate.Year == 1 && gameDate.Month == 8 &&
            gameDate.Day == 19 && SchedulingManager.Instance.GameTime >= 5 &&
            !executed)
        {
            executed = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Course Select");
    }

    public override void EventEnded()
    {
        GameManager.Instance.Course = ConversationManager.Instance.GetParameter("Course");
        GameManager.Instance.SetSelectedSubject(ScheduleType.KoreanHistory);

        if (ConversationManager.Instance.GetParameter("Economy") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.Economy);
        }
        if(ConversationManager.Instance.GetParameter("LawAndPolitics") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.LawAndPolitics);
        }
        if(ConversationManager.Instance.GetParameter("SocietyAndCulture") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.SocietyAndCulture);
        }
        if(ConversationManager.Instance.GetParameter("WorldHistory") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.WorldHistory);
        }
        if(ConversationManager.Instance.GetParameter("Ethics") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.Ethics);
        }
        if(ConversationManager.Instance.GetParameter("Geography") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.Geography);
        }
        if(ConversationManager.Instance.GetParameter("Physics") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.Physics);
        }
        if(ConversationManager.Instance.GetParameter("LifeScience") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.LifeScience);
        }
        if(ConversationManager.Instance.GetParameter("EarthScience") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.EarthScience);
        }
        if(ConversationManager.Instance.GetParameter("Chemistry") == 1)
        {
            GameManager.Instance.SetSelectedSubject(ScheduleType.Chemistry);
        }

        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
