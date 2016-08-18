using UnityEngine;
using System.Collections;
using System;

public class SchedulePartTime : Schedule
{
    public override void Init()
    {
        type = ScheduleType.Parttime;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
    }

    public override void Effect(Schedule obj)
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 2.3f);

        GameManager.Instance.Money += 3;
    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 3);

        GameManager.Instance.Money += 1;
    }
}
