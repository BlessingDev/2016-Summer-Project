using UnityEngine;
using System.Collections;
using System;

public class GameEventSleepTooLess : GameEvent {

    public override void Init()
    {
        eventName = "Sleep Too Less";
    }

    public override bool ConditionCheck()
    {
        // 수정
        return false;
    }

    public override void ExecuteEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Sleep Too Less");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.AddGameTime(UnityEngine.Random.Range(3, 7));
        SchedulingManager.Instance.initTime = false;
        float val;
        GameManager.Instance.GetParameter("Stress", out val);
        GameManager.Instance.SetParameter("Stress", val - UnityEngine.Random.Range(5, 30));
        GameManager.Instance.ScheduleExecute();
    }
}
