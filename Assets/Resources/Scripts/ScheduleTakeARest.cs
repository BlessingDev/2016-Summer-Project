using UnityEngine;
using System.Collections;
using System;

public class ScheduleTakeARest : Schedule
{

	// Use this for initialization
	void Start ()
    {
        type = ScheduleType.TakeARest;
	}

    public override void Effect(Schedule obj)
    {
        GameManager.Instance.Stress -= 1;
        Debug.Log("Take a rest Effected!");
    }
}
