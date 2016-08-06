using UnityEngine;
using System.Collections;
using System;

public class ScheduleArt : Schedule
{
    public override void Init()
    {
        base.Init();
        type = ScheduleType.Art;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Art);
    }

    public override void Effect(Schedule obj)
    {
        SchedulingManager.Instance.AddParameter("Art", 1f);
        SchedulingManager.Instance.AddParameter("Stress", 0.7f);
    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameter("Stress", 1f);
    }
}
