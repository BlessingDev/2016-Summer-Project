using UnityEngine;
using System.Collections;
using System;

public class ScheduleMovie : Schedule
{
    public override void Init()
    {
        type = ScheduleType.Movie;
        ratable = false;
        categories.Add(ParameterCategory.Stress);
    }

    public override void Effect(Schedule obj)
    {
        int val = UnityEngine.Random.Range(-3, -6);

        SchedulingManager.Instance.AddParameterAndShowText("Stress", val);
    }
}
