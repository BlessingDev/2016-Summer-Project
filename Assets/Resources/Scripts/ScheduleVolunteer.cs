using UnityEngine;
using System.Collections;
using System;

public class ScheduleVolunteer : Schedule
{
    public override void Init()
    {
        base.Init();
        type = ScheduleType.Volunteer;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Volunteer);
    }

    public override void Effect(Schedule obj)
    {
        Debug.Log("Volunteer Effected");
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 2);
        SchedulingManager.Instance.AddParameterAndShowText("Volunteer", 1);

    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 2);
    }
}
