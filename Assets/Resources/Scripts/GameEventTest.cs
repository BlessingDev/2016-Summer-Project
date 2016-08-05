using UnityEngine;
using System.Collections;
using System;

public class GameEventTest : GameEvent
{
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
