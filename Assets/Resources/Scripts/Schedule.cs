using UnityEngine;
using System.Collections;

public enum ScheduleType
{
    TakeARest,
    English,
    BasicMath,
    BasicMath2
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
	void Start()
    {
        ended = false;
	}

    public abstract void Effect(Schedule obj);
}
