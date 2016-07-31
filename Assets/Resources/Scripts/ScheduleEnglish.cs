using UnityEngine;
using System.Collections;
using System;

public class ScheduleEnglish : Schedule
{
    public override void Init()
    {
        base.Init();
        ratable = true;
        type = ScheduleType.English;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.English);
    }

    public override void Effect(Schedule obj)
    {
        Debug.Log("English Effected");
        GameManager.Instance.SetParameter("Stress", 
            GameManager.Instance.GetParameter("Stress") + 1);
        GameManager.Instance.SetParameter("English",
            GameManager.Instance.GetParameter("English") + 1);

        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.English, 1);

    }

    public override void Failed()
    {
        GameManager.Instance.SetParameter("Stress",
           GameManager.Instance.GetParameter("Stress") + 1);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 1);
    }
}
