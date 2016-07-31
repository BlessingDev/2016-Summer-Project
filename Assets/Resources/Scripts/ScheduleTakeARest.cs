using UnityEngine;
using System.Collections;
using System;

public class ScheduleTakeARest : Schedule
{
    public override void Init()
    {
        base.Init();
        type = ScheduleType.TakeARest;
    }

    public override void Effect(Schedule obj)
    {
        GameManager.Instance.SetParameter("Stress", GameManager.Instance.GetParameter("Stress") - 1);
        Debug.Log("Take a rest Effected!");
    }
}
