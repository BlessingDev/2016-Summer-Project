using UnityEngine;
using System.Collections;

public abstract class GameEvent : MonoBehaviour
{
    protected string eventName;
    public string EventName
    {
        get
        {
            return eventName;
        }
    }

    void Start()
    {
        Init();
    }

    public virtual void Init()
    {

    }
    public virtual bool EndConditionCheck()
    {
        return false;
    }

    public abstract bool ConditionCheck();
    public abstract void ExecuteEvent();
    public abstract void EventEnded();
}
