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
        if(Input.GetKeyDown(KeyCode.X))
        {
            ExamDate();
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            GameManager.Instance.Money += 40;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            CourseSelect();
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            ConversationManager.Instance.StartConversationEvent("Vacation Ceremony");
            EventManager.Instance.SetCurEvent("Vacation Ceremony");
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            OpeningCeremony();  // 문이과 선택과목이 반영되도록
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            ConversationManager.Instance.StartConversationEvent("Sleep Too Less");
            EventManager.Instance.SetCurEvent("Sleep Too Less");
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            ConversationManager.Instance.StartConversationEvent("Go Out PC");
            EventManager.Instance.SetCurEvent("Go Out PC");
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
        }

        if(Input.GetKey(KeyCode.Alpha1))
        {
            // 선거 후보 등록
            GameManager.Instance.GameDate = new Date(2, 6, 8);
        }
        if(Input.GetKey(KeyCode.Alpha2))
        {
            ConversationManager.Instance.StartConversationEvent("Chat Cafeteria");
            EventManager.Instance.SetCurEvent("Chat Cafeteria");
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            ConversationManager.Instance.StartConversationEvent("Chat Competition");
            EventManager.Instance.SetCurEvent("Chat Competition");
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            ConversationManager.Instance.StartConversationEvent("Chat Library");
            EventManager.Instance.SetCurEvent("Chat Library");
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            ConversationManager.Instance.StartConversationEvent("Chat Newspaper");
            EventManager.Instance.SetCurEvent("Chat Newspaper");
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            ConversationManager.Instance.StartConversationEvent("Election");
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            ConversationManager.Instance.StartConversationEvent("Go Out Movie");
            EventManager.Instance.SetCurEvent("Go Out Movie");
        }
        if (Input.GetKey(KeyCode.Alpha8))
        {
            //ConversationManager.Instance.StartConversationEvent("Political View Presentation");
        }
        if (Input.GetKey(KeyCode.Alpha9))
        {
            ConversationManager.Instance.StartConversationEvent("Teacher Help");
        }
        if (Input.GetKey(KeyCode.Alpha0))
        {
            ConversationManager.Instance.StartConversationEvent("University Select");
            EventManager.Instance.SetCurEvent("University Select");
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

    private void CourseSelect()
    {
        ConversationManager.Instance.StartConversationEvent("Course Select");
        EventManager.Instance.SetCurEvent("Course Select");
    }

    private void OpeningCeremony()
    {
        ConversationManager.Instance.StartConversationEvent("Opening Ceremony");
        GameManager.Instance.GameDate = new Date(2, 3, 2);
        SchedulingManager.Instance.InitTime();
        SchedulingManager.Instance.AddGameTime(5);
        EventManager.Instance.SetCurEvent("Opening Ceremony");
    }

    private void GoOutEvent()
    {
        ConversationManager.Instance.StartConversationEvent("Go Out Movie");
        EventManager.Instance.SetCurEvent("Go Out Movie");
    }

    private void ExamDate()
    {
        Date gameDate = GameManager.Instance.GameDate;

        gameDate.Month = 5;
        gameDate.Day = 2;

        GameManager.Instance.GameDate = gameDate;
    }
}
