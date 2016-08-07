using UnityEngine;
using System.Collections;

public class GameEventInterviewTomorrow : GameEvent
{
    public override void Init()
    {
        eventName = "InterviewTomorrow";
    }

    public override bool ConditionCheck()
    {
        Date date = GameManager.Instance.GameDate;
        if (date.Year == 3 && date.Month == 10 && date.Day == 29)
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
        ConversationManager.Instance.StartConversationEvent("It's Interview Tomorrow");
    }

    public override void EventEnded()
    {
        GameManager.Instance.scheduleButtonType = ScheduleButtonType.Interview;
        SceneManager.Instance.ChangeScene("GameScene");
    }
}
