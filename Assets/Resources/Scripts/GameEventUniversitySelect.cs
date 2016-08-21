using UnityEngine;
using System.Collections;
using System;

public class GameEventUniversitySelect : GameEvent {

    static bool executed = false;

    public override void Init()
    {
        eventName = "University Select";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if (!executed &&
            gameDate.Year == 2 &&
            gameDate.Month == 5 &&
            gameDate.Day == 4 &&
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
        ConversationManager.Instance.StartConversationEvent("University Select");
    }

    public override void EventEnded()
    {
        if(ConversationManager.Instance.GetParameter("순천향대") == 1)
        {
            GameManager.Instance.SetParameter("순천향대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("목포대") == 1)
        {
            GameManager.Instance.SetParameter("목포대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("동의대") == 1)
        {
            GameManager.Instance.SetParameter("동의대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("수원대") == 1)
        {
            GameManager.Instance.SetParameter("수원대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("대전대") == 1)
        {
            GameManager.Instance.SetParameter("대전대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("조선대") == 1)
        {
            GameManager.Instance.SetParameter("조선대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("부경대") == 1)
        {
            GameManager.Instance.SetParameter("부경대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("광운대") == 1)
        {
            GameManager.Instance.SetParameter("광운대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("전북대") == 1)
        {
            GameManager.Instance.SetParameter("전북대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("부산대") == 1)
        {
            GameManager.Instance.SetParameter("부산대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("국민대") == 1)
        {
            GameManager.Instance.SetParameter("국민대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("중앙대") == 1)
        {
            GameManager.Instance.SetParameter("중앙대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("동국대") == 1)
        {
            GameManager.Instance.SetParameter("동국대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("고려대") == 1)
        {
            GameManager.Instance.SetParameter("고려대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("한양대") == 1)
        {
            GameManager.Instance.SetParameter("한양대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("KAIST") == 1)
        {
            GameManager.Instance.SetParameter("KAIST", 1);
        }
        else if (ConversationManager.Instance.GetParameter("포항공대") == 1)
        {
            GameManager.Instance.SetParameter("포항공대", 1);
        }
        else if (ConversationManager.Instance.GetParameter("서울대") == 1)
        {
            GameManager.Instance.SetParameter("서울대", 1);
        }

        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
