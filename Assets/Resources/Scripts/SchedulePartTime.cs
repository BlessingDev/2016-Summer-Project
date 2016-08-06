using UnityEngine;
using System.Collections;
using System;

public class SchedulePartTime : Schedule
{
    public override void Init()
    {
        type = ScheduleType.Parttime;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
    }

    public override void Effect(Schedule obj)
    {
        GameManager.Instance.SetParameter("Stress",
            GameManager.Instance.GetParameter("Stress") + 2.3f);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 2.3f);

        GameManager.Instance.Money += 3;
    }

    public override void Failed()
    {
        GameManager.Instance.SetParameter("Stress",
             GameManager.Instance.GetParameter("Stress") + 3);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 3);

        GameManager.Instance.Money += 1;
    }
}
