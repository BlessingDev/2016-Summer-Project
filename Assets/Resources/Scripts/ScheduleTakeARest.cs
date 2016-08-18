using UnityEngine;
using System.Collections;
using System;

public class ScheduleTakeARest : Schedule
{
    public override void Init()
    {
        base.Init();
        ratable = false;
        type = ScheduleType.TakeARest;
        categories.Add(ParameterCategory.Stress);
    }

    public override void Effect(Schedule obj)
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", -1);
        Debug.Log("Take a rest Effected!");
    }
}
