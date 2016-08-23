using UnityEngine;
using System.Collections;
using System;

public class GameEventTestResult : GameEvent
{
    public static bool executable = true;

    public override void Init()
    {
        eventName = "TestResult";
    }

    public override bool ConditionCheck()
    {
        Date date = GameManager.Instance.GameDate;
        Date testDate = LoadExam.Instance.LatestTestDate;

        //가장 최근의 시험으로부터 3일이 지나면
        if(date.Year == testDate.Year && 
            date.Month == testDate.Month &&
            date.Day == testDate.Day + 3 &&
            SchedulingManager.Instance.GameTime >= 17 &&
            executable)
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
        executable = false;

        ConversationManager.Instance.StartConversationEvent("Test Result");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
