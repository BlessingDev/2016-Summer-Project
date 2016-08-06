using UnityEngine;
using System.Collections;
using System;

public class ScheduleScience : Schedule {

    public override void Init()
    {
        type = ScheduleType.Science;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Science);
    }

    public override void Effect(Schedule obj)
    {
        SchedulingManager.Instance.AddParameter("Stress", 0.7f);
        SchedulingManager.Instance.AddParameter("Science", 1f);
    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameter("Stress", 1f);
    }
}
