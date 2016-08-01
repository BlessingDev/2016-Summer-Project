using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        get
        {
            return gameDate;
        }
        set
        {
            gameDate = value;
        }
    }

    private GameObject player = null;
    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    private Dictionary<ScheduleType, int> schedulesDic;
    public Dictionary<ScheduleType, int> SchedulesDic
    {
        get
        {
            return schedulesDic;
        }
    }
    private Dictionary<string, float> parameters;
    private Dictionary<string, int> parameterLimit;

    private bool executeSchedule = false;       // 스케쥴 실행이 예약되어 있는가
    [SerializeField]
    private GameObject prePausePopup = null;
    private GameObject pausePopup = null;

    private Dictionary<string, List<Animator>> animationLayer;

	// Use this for initialization
	void Start()
    {
        stress = 0;
        schedulesDic = new Dictionary<ScheduleType, int>();

        schedulesDic.Add(ScheduleType.TakeARest, -1);
        schedulesDic.Add(ScheduleType.BasicMath, 8);
        schedulesDic.Add(ScheduleType.English, 8);

        parameters = new Dictionary<string, float>();

        parameters.Add("Stress", 60);
        parameters.Add("Math", 0);
        parameters.Add("English", 0);

        parameterLimit = new Dictionary<string, int>();

        parameterLimit.Add("Stress", 100);

        animationLayer = new Dictionary<string, List<Animator>>();

        gameDate.Year = 1;
        gameDate.Month = 3;
        gameDate.Day = 2;

        if(prePausePopup == null)
        {
            Debug.LogWarning("The Prefab NOT PREPARED");
        }
    }

    void Update()
    {
        if (!pause)
        {
            SchedulingManager.Instance.update();
            EventManager.Instance.update();
            UIManager.Instance.update();
            CheckGameOver();
        }
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        animationLayer = new Dictionary<string, List<Animator>>();
        UIManager.Instance.OnLevelWasLoaded(level);
        switch (level)
        {
            case 0:
                if (executeSchedule)
                {
                    executeSchedule = false;

                    SchedulingManager.Instance.OpenSchedulePopup();


                    SchedulingManager.Instance.Progressing = true;
                }
                break;
        }
    }

    private void CheckGameOver()
    {
        if(gameDate.Year == 4 && gameDate.Month == 2 && gameDate.Day == 15)
        {
            GameOver();
        }
    }

    public void StartSchedule()
    {
        SchedulingManager.Instance.Progressing = true;
    }

    public float GetParameter(string name)
    {
        float val = 0;
        if(parameters.TryGetValue(name, out val))
        {
            return val;
        }
        else
        {
            Debug.LogError("parameters DOESN'T HAVE " + name);
            return -1;
        }
    }

    public bool SetParameter(string name, float value)
    {
        if(!parameters.ContainsKey(name))
        {
            Debug.LogError("parameters DOESN'T HAVE " + name);
            return false;
        }

        if (value < 0)
        {
            value = 0;
            Debug.LogWarning("Parameter " + name + " LowerLimit Bound");
        }

        int limit;
        if(parameterLimit.TryGetValue(name, out limit))
        {
            if (value > limit)
            {
                value = limit;
                Debug.LogWarning("Parameter " + name + " UpperLimit Bound");
            }
        }

        parameters.Remove(name);
        parameters.Add(name, value);

        return true;
    }

    public void ScheduleExecute()
    {
        executeSchedule = true;
        SceneManager.Instance.ChangeScene("GameScene");
    }

    public void PauseGame()
    {
        UIManager.Instance.SetEnableTouchLayer("Main", false);
        pause = true;

        pausePopup = Instantiate(prePausePopup);
        pausePopup.transform.parent = UIManager.Instance.Canvas.transform;
        pausePopup.transform.localPosition = Vector3.zero;
        pausePopup.transform.localScale = Vector3.one;

        SetPlayAnimator("Player", false);

        if (SchedulingManager.Instance.Progressing)
            SetPlayAnimator("CutScene", false);
    }

    public void ResumeGame()
    {
        UIManager.Instance.SetEnableTouchLayer("Main", true);
        pause = false;

        Destroy(pausePopup);
        pausePopup = null;

        SetPlayAnimator("Player", true);

        if (SchedulingManager.Instance.Progressing)
            SetPlayAnimator("CutScene", true);
    }

    public void AddAnimator(string layerName, Animator animator)
    {
        if(animationLayer.ContainsKey(layerName))
        {
            List<Animator> list;
            animationLayer.TryGetValue(layerName, out list);
            list.Add(animator);
            animationLayer.Remove(layerName);
            animationLayer.Add(layerName, list);
        }
        else
        {
            List<Animator> list = new List<Animator>();
            list.Add(animator);
            animationLayer.Add(layerName, list);
        }
    }

    public bool SetPlayAnimator(string layerName, bool play)
    {
        if(animationLayer.ContainsKey(layerName))
        {
            List<Animator> list;
            animationLayer.TryGetValue(layerName, out list);
            foreach(var iter in list)
            {
                iter.enabled = play;
            }

            return true;
        }
        else
        {
            Debug.LogWarning(layerName + " layer DOESN'T EXIST");
            return false;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Overed");
    }
}
