using UnityEngine;
using System.Collections;
using System;

public class ScheduleBasicMath1 : Schedule
{
    public override void Init()
    {
        type = ScheduleType.BasicMath;
    }

    public override void Effect(Schedule obj)
    {
        GameManager.Instance.SetParameter("Stress", GameManager.Instance.GetParameter("Stress") + 1);
        GameManager.Instance.SetParameter("BasicMath1", GameManager.Instance.GetParameter("BasicMath1") + 1);
        Debug.Log("Math Effected!");
    }
}
