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
        GameManager.Instance.SetParameter("Stress", GameManager.Instance.GetParameter("Stress") - 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, -1);
        Debug.Log("Take a rest Effected!");
    }
}
