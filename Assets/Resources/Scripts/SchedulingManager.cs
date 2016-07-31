using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public struct SteakerInfo
{
    public ScheduleType type;
    public int num;
}

public class SchedulingManager : Manager<SchedulingManager>
{
    private Dictionary<ScheduleType, GameObject> preScheduleDic;
    private Dictionary<ScheduleType, GameObject> preSteakerDic;
    private Dictionary<ParameterCategory, GameObject> preParameters;
    private Dictionary<ParameterCategory, GameObject> parameters;
    private List<List<SteakerInfo>> steakerList;
    private Dictionary<ScheduleType, GameObject> curSteakerDic;
    public Dictionary<ScheduleType, GameObject> CurSteakerDic
    {
        get
        {
            return curSteakerDic;
        }
    }
    private Schedule[] scheduleList;
    private int curTime = 1;
    private float befTime = 1;
    private float timeRate;
    private float time;
    public float GameTime
    {
        get
        {
            return time;
        }
    }

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
            SetCutSceneAnimation();
            SetParameterBar();
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
    [SerializeField]
    private GameObject preSteakerButton = null;
    private SteakerPlate steakers;

    private int curPlace = 1;
    private const int STEAKER_LIMIT = 5;
    int curSteakerPlate = 0;

    [SerializeField]
    private GameObject preStudyMethodPopup = null;
    private GameObject studyMethodPopup = null;

    private int studyMode = 2;
    public int StudyMode
    {
        get
        {
            return studyMode;
        }
        set
        {
            studyMode = value;
        }
    }

    private bool gotoMainReserved = false;
    public bool GotoMainReserved
    {
        get
        {
            return gotoMainReserved;
        }
        set
        {
            gotoMainReserved = value;
        }
    }

    [SerializeField]
    private GameObject preSchedulePopup = null;
    private SchedulePopup schedulePopup = null;

    [SerializeField]
    private GameObject preChangeNumber = null;

    // Use this for initialization
    void Start ()
    {
        preScheduleDic = new Dictionary<ScheduleType, GameObject>();
        preSteakerDic = new Dictionary<ScheduleType, GameObject>();
        steakerList = new List<List<SteakerInfo>>();
        preParameters = new Dictionary<ParameterCategory, GameObject>();

        GameObject[] objs = Resources.LoadAll<GameObject>("Prefabs/Schedules/Acts/");
        
        for(int i = 0; i < objs.Length; i += 1)
        {
            Schedule schedule = objs[i].GetComponent<Schedule>();
            schedule.Init();
            preScheduleDic.Add(schedule.Type, objs[i]);
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
            preSteakerDic.Add(schedule.Type, objs[i]);
        }

        objs = Resources.LoadAll<GameObject>("Prefabs/Parameters/");
        for(int i = 0; i < objs.Length; i += 1)
        {
            ParameterBar bar = objs[i].GetComponent<ParameterBar>();
            preParameters.Add(bar.Category, bar.gameObject);
        }

        timeRate = 3f; // 1일은 8초
        progressing = false;

        if(preOneToTwelve == null || preThirteenToTwentyFour == null ||
            preSteakerPlate == null || preSteakerButton == null ||
            preStudyMethodPopup == null || preSchedulePopup == null ||
            preChangeNumber == null)
        {
            Debug.LogWarning("The Prefab NOT PREPARED");
        }
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);
        UIManager.Instance.OnLevelWasLoaded(level);
        switch(level)
        {
            case 1:
                GameObject obj = Instantiate<GameObject>(preOneToTwelve);
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
        curSteakerDic = new Dictionary<ScheduleType, GameObject>();
        steakers = Instantiate(preSteakerPlate).GetComponent<SteakerPlate>();
        GridLayoutGroup group = steakers.GetComponent<GridLayoutGroup>();

        steakers.transform.parent = UIManager.Instance.Canvas.transform;
        steakers.transform.localPosition = new Vector2(276, 0);
        steakers.transform.localScale = Vector3.one;

        GameObject obj = Instantiate(preSteakerButton);
        obj.transform.parent = UIManager.Instance.Canvas.transform;
        obj.transform.localPosition = new Vector2(276, 0);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<ObjectFollower>().Other = steakers.transform;

        steakerList.Clear();
        var dic = GameManager.Instance.SchedulesDic;

        int i = 0;
        foreach(var iter in dic)
        {
            if (i % STEAKER_LIMIT == 0)
                steakerList.Add(new List<SteakerInfo>());
            SteakerInfo info = new SteakerInfo();
            info.type = iter.Key;
            info.num = iter.Value;

            steakerList[i / STEAKER_LIMIT].Add(info);

            i += 1;
        }

        for (int j = 0; j < steakerList[0].Count; j += 1)
        {
            if (preSteakerDic.TryGetValue(steakerList[0][j].type, out obj))
            {
                GameObject steObj = Instantiate(obj);
                steObj.transform.parent = steakers.transform;
                steObj.transform.localScale = Vector3.one;

                Steaker ste = steObj.GetComponent<Steaker>();
                ste.Num = steakerList[0][j].num;
                curSteakerDic.Add(steakerList[0][j].type, steObj);
            }
            else
            {
                Debug.LogError("The Schedule DOESN'T EXIST");
            }
        }

        int useLength = steakerList[0].Count * 50 + (steakerList[0].Count - 1) * 70;

        group.padding.top = (528 - useLength) / 2;
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
        if(scheduleList[time - 1] != null)
        {
            DeleteAt(time);
        }

        GameObject obj = null;
        if(preScheduleDic.TryGetValue(type, out obj))
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
            if(gotoMainReserved)
            {
                gotoMainReserved = false;
                progressing = false;

                CloseSchedulePopup();
            }

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
            ScheduleExecute();

            curTime += 1;
            SetCutSceneAnimation();
            SetParameterBar();
        }
    }

    void SetCutSceneAnimation()
    {
        int aniType = 0;
        switch (scheduleList[curTime - 1].Type)
        {
            case ScheduleType.TakeARest:
                aniType = 1;
                break;
            case ScheduleType.BasicMath:
            case ScheduleType.English:
                aniType = 2;
                break;
        }

        schedulePopup.SetAnimationType(aniType);
    }

    void SetParameterBar()
    {
        parameters = new Dictionary<ParameterCategory, GameObject>();
        schedulePopup.InitParameters();
        var list = scheduleList[curTime - 1].Categories;
        foreach(var iter in list)
        {
            GameObject obj;
            if(preParameters.TryGetValue(iter, out obj))
            {
                obj = Instantiate(obj);
                schedulePopup.AddParameter(obj);
                parameters.Add(iter, obj);
            }
            else
            {
                Debug.LogError(iter + " is NOT found");
                return;
            }
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
                if(preSteakerDic.TryGetValue(scheduleList[time - 1].Type, out obj))
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

    public void ResetSchedules()
    {
        oneToTwelve.Reset();
        thirteenToTwentyFour.Reset();
    }

    public void SteakerPlateUpper()
    {
        if(curSteakerPlate < steakerList.Count - 1)
        {
            curSteakerDic = new Dictionary<ScheduleType, GameObject>();

            for(int i = 0; i < steakers.transform.childCount; i += 1)
            {
                Destroy(steakers.transform.GetChild(i).gameObject);
            }

            curSteakerPlate += 1;

            GameObject obj;
            for(int i = 0; i < steakerList[curSteakerPlate].Count; i += 1)
            {
                if (preSteakerDic.TryGetValue(steakerList[curSteakerPlate][i].type, out obj))
                {
                    GameObject steObj = Instantiate(obj);
                    steObj.transform.parent = steakers.transform;
                    steObj.transform.localScale = Vector3.one;

                    Steaker ste = steObj.GetComponent<Steaker>();
                    ste.Num = steakerList[curSteakerPlate][i].num;
                    curSteakerDic.Add(steakerList[curSteakerPlate][i].type, steObj);
                }
                else
                {
                    Debug.LogError("The Schedule DOESN'T EXIST");
                }
            }
        }
        else
        {
            Debug.LogWarning("Can't Move Steaker");
        }
    }

    public void SteakerPlateBackward()
    {
        if(curSteakerPlate > 1)
        {
            curSteakerDic = new Dictionary<ScheduleType, GameObject>();

            for (int i = 0; i < steakers.transform.childCount; i += 1)
            {
                Destroy(steakers.transform.GetChild(i).gameObject);
            }

            curSteakerPlate -= 1;

            GameObject obj;
            for (int i = 0; i < steakerList[curSteakerPlate].Count; i += 1)
            {
                if (preSteakerDic.TryGetValue(steakerList[curSteakerPlate][i].type, out obj))
                {
                    GameObject steObj = Instantiate(obj);
                    steObj.transform.parent = steakers.transform;
                    steObj.transform.localScale = Vector3.one;

                    Steaker ste = steObj.GetComponent<Steaker>();
                    ste.Num = steakerList[curSteakerPlate][i].num;
                    curSteakerDic.Add(steakerList[curSteakerPlate][i].type, steObj);
                }
                else
                {
                    Debug.LogError("The Schedule DOESN'T EXIST");
                }
            }
        }
        else
        {
            Debug.LogWarning("Can't Move Steaker");
        }
    }

    public void OpenStudyMethodPopup()
    {
        studyMethodPopup = Instantiate(preStudyMethodPopup);
        studyMethodPopup.transform.parent = UIManager.Instance.Canvas.transform;
        studyMethodPopup.transform.localPosition = Vector3.zero;
        studyMethodPopup.transform.localScale = Vector3.one;

        UIManager.Instance.SetEnableTouchLayer("Main", false);
        UIManager.Instance.SetEnableTouchLayer("Steaker", false);
    }

    public void CloseStudyMethodPopup()
    {
        Destroy(studyMethodPopup);
        studyMethodPopup = null;

        UIManager.Instance.SetEnableTouchLayer("Main", true);
        UIManager.Instance.SetEnableTouchLayer("Steaker", true);
    }

    public void OpenSchedulePopup()
    {
        GameObject obj = Instantiate(preSchedulePopup);
        obj.transform.parent = UIManager.Instance.Canvas.transform;
        obj.transform.localPosition = new Vector2(-150, -20);
        obj.transform.localScale = Vector3.one;

        schedulePopup = obj.GetComponent<SchedulePopup>();
    }

    public void CloseSchedulePopup()
    {
        Destroy(schedulePopup.gameObject);
        schedulePopup = null;
    }

    // 스케줄이 성공했는지 검사하고 후속 조치를 취한다.
    public void ScheduleExecute()
    {
        if(scheduleList[curTime - 1].IsRatable)
        {
            float percentage = 100 - GameManager.Instance.GetParameter("Stress");

            float val = Random.value * 100;

            if (val <= percentage)
            {
                scheduleList[curTime - 1].Effect(scheduleList[curTime - 1]);
                Debug.Log("Schedule Succed");
            }
            else
            {
                scheduleList[curTime - 1].Failed();
                Debug.Log("Schedule Failed");
            }
        }
        else
        {
            scheduleList[curTime - 1].Effect(scheduleList[curTime - 1]);
        }
    }

    public bool ShowChangeText(ParameterCategory category, float val)
    {
        if(parameters.ContainsKey(category))
        {
            GameObject obj;
            parameters.TryGetValue(category, out obj);
            GameObject text = Instantiate(preChangeNumber);
            text.transform.SetParent(UIManager.Instance.Canvas.transform);
            text.transform.localScale = Vector3.one;
            string numText = (val >= 0) ? "+" + val.ToString() : val.ToString();
            text.GetComponent<Text>().text = numText;
            text.transform.GetChild(0).GetComponent<Text>().text = numText;

            text.transform.position = obj.transform.position;
            text.transform.localPosition = new Vector2(text.transform.localPosition.x, text.transform.localPosition.y + 60);

            StartCoroutine(CorChangeVal(text, category));
            return true;
        }
        else
        {
            Debug.LogWarning(category + " parameter DOESN'T EXIST");
            return false;
        }
    }

    private IEnumerator CorChangeVal(GameObject obj, ParameterCategory category)
    {
        Text text = obj.GetComponent<Text>();
        Color color = text.color;
        color.a = 0;
        text.color = color;

        float alphaSpeed = 1f;
        float ySpeed = 10f;

        while(true)
        {
            color = text.color;
            color.a += alphaSpeed * Time.smoothDeltaTime;
            text.color = color;

            if(color.a >= 1f || !parameters.ContainsKey(category))
            {
                Destroy(obj);
                break;
            }

            Vector2 pos = obj.transform.localPosition;
            pos.y += ySpeed * Time.smoothDeltaTime;
            obj.transform.localPosition = pos;

            yield return null;
        }
    }
}
