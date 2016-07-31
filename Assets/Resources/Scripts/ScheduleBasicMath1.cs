using UnityEngine;
using System.Collections;
using System;

public class ScheduleBasicMath1 : Schedule
{
    public override void Init()
    {
        type = ScheduleType.BasicMath;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Math);
    }

    public override void Effect(Schedule obj)
    {
        GameManager.Instance.SetParameter("Stress", GameManager.Instance.GetParameter("Stress") + 1);
        GameManager.Instance.SetParameter("Math", GameManager.Instance.GetParameter("Math") + 1);
        Debug.Log("Math Effected!");
    }
}
