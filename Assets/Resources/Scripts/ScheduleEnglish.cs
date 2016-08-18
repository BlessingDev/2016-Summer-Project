using UnityEngine;
using System.Collections;
using System;

public class ScheduleEnglish : Schedule
{
    public override void Init()
    {
        base.Init();
        ratable = true;
        type = ScheduleType.English;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.English);
    }

    public override void Effect(Schedule obj)
    {
        Debug.Log("English Effected");
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 1);
        SchedulingManager.Instance.AddParameterAndShowText("English", 1);

    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 1);
    }
}
