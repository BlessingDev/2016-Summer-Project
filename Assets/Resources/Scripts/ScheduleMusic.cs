using UnityEngine;
using System.Collections;
using System;

public class ScheduleMusic : Schedule
{
    public override void Init()
    {
        type = ScheduleType.Music;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Music);
    }

    public override void Effect(Schedule obj)
    {
        SchedulingManager.Instance.AddParameter("Stress", 0.7f);
        SchedulingManager.Instance.AddParameter("Music", 1f);
    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameter("Stress", 1);
    }
}
