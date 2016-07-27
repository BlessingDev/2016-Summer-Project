using UnityEngine;
using System.Collections;

public class ScheduleTest : MonoBehaviour
{
    private int time;

	// Use this for initialization
	void Start ()
    {
        time = 1;
	}
	
    public void OnPress()
    {
        SchedulingManager.Instance.SetSchedule(time, ScheduleType.TakeARest);
        time = (time) % 24 + 1;
    }
}
