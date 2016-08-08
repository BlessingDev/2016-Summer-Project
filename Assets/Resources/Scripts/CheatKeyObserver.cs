using UnityEngine;
using System.Collections;

public class CheatKeyObserver : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Date date = GameManager.Instance.GameDate;
            date.Day += 1;
            GameManager.Instance.GameDate = date;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            AutoSchedule();
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            InterviewDate();
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            EndDate();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            ConvTest();
        }
	}

    private void AutoSchedule()
    {
        for(int i = 1; i <= 24; i += 1)
        {
            if (i <= 12)
                SchedulingManager.Instance.SetSchedule(i, ScheduleType.TakeARest);
            else
                SchedulingManager.Instance.SetSchedule(i, ScheduleType.BasicMath);
        }
    }

    private void InterviewDate()
    {
        Date date = GameManager.Instance.GameDate;
        date.Year = 3;
        date.Month = 10;
        date.Day = 29;
        GameManager.Instance.GameDate = date;
    }

    private void EndDate()
    {
        Date date = GameManager.Instance.GameDate;
        date.Year = 3;
        date.Month = 12;
        date.Day = 1;
        GameManager.Instance.GameDate = date;
    }

    private void ConvTest()
    {
        ConversationManager.Instance.StartConversationEvent("University Select");
    }
}
