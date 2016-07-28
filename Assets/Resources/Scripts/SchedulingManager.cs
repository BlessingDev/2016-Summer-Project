using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SchedulingManager : Manager<SchedulingManager>
{
    private Dictionary<ScheduleType, GameObject> scheduleDic;
    private Dictionary<ScheduleType, GameObject> steakerDic;
    private Schedule[] scheduleList;
    private int curTime = 1;
    private float befTime = 1;
    private float timeRate;
    private float time;

    private bool progressing;               // 현재 스케줄이 진행되고 있는가
    public bool Progressing
    {
        get
        {
            return progressing;
        }
        set
        {
            curTime = 1;
            befTime = 0;
            time = 0;
            progressing = value;
        }
    }

    [SerializeField]
    private GameObject preOneToTwelve = null;
    [SerializeField]
    private GameObject preThirteenToTwentyFour = null;
    private SlotCollector oneToTwelve;
    private SlotCollector thirteenToTwentyFour;

    [SerializeField]
    private GameObject preSteakerPlate = null;
    private SteakerPlate steakers;

    private int curPlace = 1;

	// Use this for initialization
	void Start ()
    {
        scheduleDic = new Dictionary<ScheduleType, GameObject>();
        steakerDic = new Dictionary<ScheduleType, GameObject>();

        GameObject[] objs = Resources.LoadAll<GameObject>("Prefabs/Schedules/Acts/");
        
        for(int i = 0; i < objs.Length; i += 1)
        {
            Schedule schedule = objs[i].GetComponent<Schedule>();
            schedule.TypeInit();
            scheduleDic.Add(schedule.Type, objs[i]);
        }

        scheduleList = new Schedule[24];
        for(int i = 0; i < 24; i += 1)
        {
            scheduleList[i] = null;
        }

        objs = Resources.LoadAll<GameObject>("Prefabs/Schedules/Steakers/");
        for (int i = 0; i < objs.Length; i += 1)
        {
            SchedulingDragHandler schedule = objs[i].GetComponent<SchedulingDragHandler>();
            steakerDic.Add(schedule.Type, objs[i]);
        }

        timeRate = 24f / 180f; // 1일은 3분
        progressing = false;

        if(preOneToTwelve == null || preThirteenToTwentyFour == null ||
            preSteakerPlate == null)
        {
            Debug.LogWarning("The Prefab NOT PREPARED");
        }
    }

    void OnLevelWasLoaded(int level)
    {
        UIManager.Instance.OnLevelWasLoaded(level);
        switch(level)
        {
            case 1:
                GameObject obj= Instantiate<GameObject>(preOneToTwelve);
                obj.transform.parent = UIManager.Instance.Canvas.transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = new Vector2(-320, 0);
                oneToTwelve = obj.GetComponent<SlotCollector>();

                obj = Instantiate<GameObject>(preThirteenToTwentyFour);
                obj.transform.parent = UIManager.Instance.Canvas.transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = new Vector2(320, 0);
                thirteenToTwentyFour = obj.GetComponent<SlotCollector>();
                thirteenToTwentyFour.Start();
                thirteenToTwentyFour.SetImagesEnable(false);

                curPlace = 1;

                MakeSteakerBook();
                break;
        }
    }

    void MakeSteakerBook()
    {
        steakers = Instantiate(preSteakerPlate).GetComponent<SteakerPlate>();
        GridLayoutGroup group = steakers.GetComponent<GridLayoutGroup>();

        steakers.transform.parent = UIManager.Instance.Canvas.transform;
        steakers.transform.localPosition = new Vector2(276, 0);
        steakers.transform.localScale = Vector3.one;

        var dic = GameManager.Instance.SchedulesDic;

        int useLength = dic.Count * 50 + (dic.Count - 1) * 70;

        group.padding.top = (678 - useLength) / 2;

        foreach(var iter in dic)
        {
            GameObject obj;
            if(steakerDic.TryGetValue(iter.Key, out obj))
            {
                GameObject ste = Instantiate(obj);
                ste.transform.parent = steakers.transform;
                ste.transform.localScale = Vector3.one;

                ste.GetComponent<Steaker>().Num = iter.Value;
            }
            else
            {
                Debug.LogError("The Schedule DOESN'T EXIST");
            }
        }
    }

    //
    // 요약:
    //    스케줄을 설정하기 위한 메서드
    //
    // 매개변수:
    //    time: 1~24시까지의 시간 중 스케줄할 시간
    //
    //    type: 스케줄의 타입
    //
    public void SetSchedule(int time, ScheduleType type)
    {
        GameObject obj = null;
        if(scheduleDic.TryGetValue(type, out obj))
        {
            GameObject ins = Instantiate<GameObject>(obj);

            scheduleList[time - 1] = ins.GetComponent<Schedule>();
        }
        else
        {
            Debug.LogError("Type " + type + " object DOESN'T EXIST on prefab table");
        }
    }

    public void update()
    {
        if(progressing)
        {
            TimeUpdate();
            ScheduleUpdate();
        }
    }

    void TimeUpdate()
    {
        time += Time.smoothDeltaTime * timeRate;
        if(time >= 24)
        {
            curTime = 1;
            befTime = 0;
            Date date = GameManager.Instance.GameDate;
            date.Day += 1;
            GameManager.Instance.GameDate = date;

            float dif = time - 24;
            time = dif;
        }
    }

    void ScheduleUpdate()
    {
        if(time - befTime >= 1.0f)
        {
            befTime = time;
            try
            {
                scheduleList[curTime - 1].Effect(scheduleList[curTime - 1]);
            }
            catch(UnityException exc)
            {
                Debug.LogError(exc.Message + " at SchedulingManager.ScheduleUpdate1");
            }

            curTime += 1;
        }
    }

    public ScheduleType GetTypeAt(int time)
    {
        if(time >= 1 && time <= 24)
        {
            if(scheduleList[time - 1])
            {
                return scheduleList[time - 1].Type;
            }
            else
            {
                Debug.LogWarning("the Schedule DOESN'T EXIST");
                return (ScheduleType)0;
            }
        }
        else
        {
            Debug.LogError("time ISN'T 1~24");
            return (ScheduleType)0;
        }
    }

    public bool DeleteAt(int time)
    {
        if (time >= 1 && time <= 24)
        {
            if (scheduleList[time - 1])
            {
                Destroy(scheduleList[time - 1].gameObject);
                scheduleList[time - 1] = null;
                return true;
            }
            else
            {
                Debug.LogWarning("the Schedule DOESN'T EXIST");
                return false;
            }
        }
        else
        {
            Debug.LogError("time ISN'T 1~24");
            return false;
        }
    }

    public GameObject GetSteaker(int time)
    {
        if (time >= 1 && time <= 24)
        {
            if (scheduleList[time - 1])
            {
                GameObject obj = null;
                if(steakerDic.TryGetValue(scheduleList[time - 1].Type, out obj))
                {
                    return Instantiate<GameObject>(obj);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Debug.LogWarning("The Schedule DOESN'T EXIST");
                return null;
            }
        }
        else
        {
            Debug.LogError("time ISN'T 1~24");
            return null;
        }
    }

    public void MoveScheduleList(int direction)
    {
        if(curPlace == direction)
        {
            steakers.MovePlate(curPlace);
        }
    }

    public void MoveScheduleListEnded()
    {
        if(curPlace == 1)
        {
            oneToTwelve.SetImagesEnable(false);
            thirteenToTwentyFour.SetImagesEnable(true);
        }
        else
        {
            oneToTwelve.SetImagesEnable(true);
            thirteenToTwentyFour.SetImagesEnable(false);
        }

        curPlace = (curPlace == 1) ? 2 : 1;
    }
}
