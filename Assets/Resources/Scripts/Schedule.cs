using UnityEngine;
using System.Collections;

public enum ScheduleType
{
    TakeARest,
    English,
    BasicMath
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

    private bool ended;
    public bool IsEnded
    {
        get
        {
            return ended;
        }
    }

	// Use this for initialization
	protected void Start()
    {
        DontDestroyOnLoad(this);
	}

    public abstract void TypeInit();

    public abstract void Effect(Schedule obj);
}
