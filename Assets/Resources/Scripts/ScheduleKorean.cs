using UnityEngine;
using System.Collections;
using System;

public class ScheduleKorean : Schedule
{
    public override void Init()
    {
        base.Init();
        type = ScheduleType.Korean;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Korean);
    }

    public override void Effect(Schedule obj)
    {
        Debug.Log("Korean Effected");
        GameManager.Instance.SetParameter("Stress",
            GameManager.Instance.GetParameter("Stress") + 1);
        GameManager.Instance.SetParameter("Korean",
            GameManager.Instance.GetParameter("Korean") + 1);

        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Korean, 1);

    }

    public override void Failed()
    {
        GameManager.Instance.SetParameter("Stress",
           GameManager.Instance.GetParameter("Stress") + 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 1);
    }
}
