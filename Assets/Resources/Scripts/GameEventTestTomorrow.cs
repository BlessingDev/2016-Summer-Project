using UnityEngine;
using System.Collections;

public class GameEventTestTomorrow : GameEvent
{

    public override void Init()
    {
        eventName = "Test Tomorrow";
    }

    public override bool ConditionCheck()
    {
        Date date = GameManager.Instance.GameDate;

        if (date.Month == 5 && date.Day == 2)
            return true;
        else if (date.Month == 6 && date.Day == 28)
            return true;
        else
            return false;
    }

    public override void ExecuteEvent()
    {
        DontDestroyOnLoad(gameObject);
        ConversationManager.Instance.StartConversationEvent("It's Test Tomorrow");
    }

    public override void EventEnded()
    {
        GameManager.Instance.scheduleButtonType = ScheduleButtonType.Test;
        SceneManager.Instance.ChangeScene("GameScene");
    }
}
