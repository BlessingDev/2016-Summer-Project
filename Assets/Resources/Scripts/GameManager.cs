using UnityEngine;
using System.Collections;

public struct Date
{
    private int year;
    public int Year
    {
        get;
        set;
    }
    private int month;
    public int Month
    {
        get
        {
            return month;
        }

        set
        {
            month = value;
            if(month >= 13)
            {
                year += 1;
                month = 1;
            }
        }
    }
    private int day;
    public int Day
    {
        get
        {
            return day;
        }

        set
        {
            day = value;

            int lastDay = 0;
            switch(month)
            {
                case 1:
                    lastDay = 31;
                    break;
                case 2:
                    lastDay = 29;
                    break;
                case 3:
                    lastDay = 31;
                    break;
                case 4:
                    lastDay = 30;
                    break;
                case 5:
                    lastDay = 31;
                    break;
                case 6:
                    lastDay = 30;
                    break;
                case 7:
                    lastDay = 31;
                    break;
                case 8:
                    lastDay = 31;
                    break;
                case 9:
                    lastDay = 30;
                    break;
                case 10:
                    lastDay = 31;
                    break;
                case 11:
                    lastDay = 30;
                    break;
                case 12:
                    lastDay = 31;
                    break;
                default:
                    Debug.LogError("Invalid Month", GameManager.Instance);
                    break;
            }

            if(day >= lastDay)
            {
                day = 1;
                Month += 1;
            }
        }
    }
}

public class GameManager : Manager<GameManager>
{

    private bool pause = false;
    public bool IsPause
    {
        get
        {
            return pause;
        }
    }                    // 누적 시간

    private int stress;
    public int Stress
    {
        get
        {
            return stress;
        }

        set
        {
            if(value <= 100 && value >= 0)
            {
                stress = value;
            }
            else
            {
                Debug.LogWarning("Stress.value invalid value");
            }
        }
    }

    private Date gameDate;
    public Date GameDate
    {
        get;
        set;
    }

    private GameObject player = null;
    public GameObject Player
    {
        get
        {
            return player;
        }
    }

	// Use this for initialization
	void Start()
    {
        stress = 0;
	}

    void Update()
    {
        if (!pause)
        {
            SchedulingManager.Instance.update();
        }
    }

    public void StartSchedule()
    {
        SchedulingManager.Instance.Progressing = true;
    }
}
