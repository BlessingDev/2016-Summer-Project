using UnityEngine;
using System.Collections;
using System;

public class ScheduleMathCompetition : Schedule
{
    public override void Init()
    {
        type = ScheduleType.MathCompetition;
        ratable = false;
        categories.Add(ParameterCategory.Stress);
    }

    public override void Effect(Schedule obj)
    {
        int val = UnityEngine.Random.Range(1, 5);

        SchedulingManager.Instance.AddParameter("Stress", val);
    }
}
