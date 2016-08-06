using UnityEngine;
using System.Collections;
using System;

public class ScheduleWorldHistory : Schedule
{
    public override void Init()
    {
        type = ScheduleType.WorldHistory;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Social);
    }

    public override void Effect(Schedule obj)
    {
        GameManager.Instance.SetParameter("Stress",
            GameManager.Instance.GetParameter("Stress") + 0.7f);
        GameManager.Instance.SetParameter("Social",
            GameManager.Instance.GetParameter("Social") + 1f);

        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 0.7f);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Social, 1f);
    }

    public override void Failed()
    {
        GameManager.Instance.SetParameter("Stress",
             GameManager.Instance.GetParameter("Stress") + 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 1);
    }
}
