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
        GameManager.Instance.SetParameter("Stress", GameManager.Instance.GetParameter("Stress") + 0.7f);
        GameManager.Instance.SetParameter("Math", GameManager.Instance.GetParameter("Math") + 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 0.7f);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Math, 1);

        Debug.Log("Math Effected!");
    }

    public override void Failed()
    {
        GameManager.Instance.SetParameter("Stress",
           GameManager.Instance.GetParameter("Stress") + 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 1);
    }
}
