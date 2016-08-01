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
        GameManager.Instance.SetParameter("Stress",
            GameManager.Instance.GetParameter("Stress") + 2);
        GameManager.Instance.SetParameter("Volunteer",
            GameManager.Instance.GetParameter("Volunteer") + 1);

        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 2);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Volunteer, 1);

    }

    public override void Failed()
    {
        GameManager.Instance.SetParameter("Stress",
           GameManager.Instance.GetParameter("Stress") + 2);
        SchedulingManager.Instance.ShowChangeText(ParameterCategory.Stress, 2);
    }
}
