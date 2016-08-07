using UnityEngine;
using System.Collections;
using System;

public class GameEventGameOverStopSchedule : GameEvent {

    Date endDate;
    bool executed = false;

    public override void Init()
    {
        eventName = "GameOverStopSchedule";
    }

    public override bool ConditionCheck()
    {
        bool chk = GameManager.Instance.GameOverCheck();
        if(chk)
        {
            endDate = GameManager.Instance.GameDate;
        }

        return chk && !executed;
    }

    public override bool EndConditionCheck()
    {
        if(SchedulingManager.Instance.GameTime >= 23.9f)
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
        SchedulingManager.Instance.StopScheduleAndLock();
    }

    public override void EventEnded()
    {
        GameManager.Instance.scheduleButtonType = ScheduleButtonType.End;
        GameManager.Instance.InitScheduleButton();
    }
}
