using UnityEngine;
using System.Collections;

public enum ScheduleType
{
    TakeARest,
    English,
    BasicMath,
    Korean
}

public abstract class Schedule : MonoBehaviour
{
    protected ScheduleType type;
    public ScheduleType Type
    {
        get
        {
            return type;
        }
    }

    private bool limited = false;       // 편집 제한된 스케줄인가
    public bool IsLimited
    {
        get
        {
            return limited;
        }
        set
        {
            Debug.Log("limited is Changed");
            limited = value;
        }
    }

    private bool inited = false;

	// Use this for initialization
	protected void Start()
    {
        if(!inited)
            Init();
        DontDestroyOnLoad(this);
	}

    public virtual void Init()
    {
        inited = true;
    }

    public abstract void Effect(Schedule obj);
}
