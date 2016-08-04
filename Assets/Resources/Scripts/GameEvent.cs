using UnityEngine;
using System.Collections;

public abstract class GameEvent : MonoBehaviour
{
    void Start()
    {
        Init();
    }

    public virtual void Init()
    {

    }

    public abstract bool ConditionCheck();
    public abstract void ExecuteEvent();
    public abstract void EventEnded();
}
