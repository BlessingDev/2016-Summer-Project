using UnityEngine;
using System.Collections;
using System;

public class ScheduleKorean : Schedule
{
    public override void Init()
    {
        base.Init();
        type = ScheduleType.Korean;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Korean);
    }

    public override void Effect(Schedule obj)
    {
        Debug.Log("Korean Effected");
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 1);
        SchedulingManager.Instance.AddParameterAndShowText("Korean", 1);

    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 1);
    }
}
