using UnityEngine;
using System.Collections;
using System;

public class ScheduleBasicMath1 : Schedule
{
    public override void Init()
    {
        type = ScheduleType.BasicMath;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Math);
    }

    public override void Effect(Schedule obj)
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 0.7f);
        SchedulingManager.Instance.AddParameterAndShowText("Math", 1);

        Debug.Log("Math Effected!");
    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 1);
    }
}
