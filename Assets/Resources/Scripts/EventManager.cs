using UnityEngine;
using System.Collections;

public class EventManager : Manager<EventManager>
{
    GameEvent[] events;
    private GameEvent curEvent;

    // Use this for initialization
    void Start ()
    {
        events = FindObjectsOfType<GameEvent>();
	}

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);
        events = FindObjectsOfType<GameEvent>();
    }

    public void update()
    {
        if(SchedulingManager.Instance.Progressing)
        {
            for(int i = 0; i < events.Length; i += 1)
            {
                if(events[i].ConditionCheck())
                {
                    curEvent = events[i];
                    events[i].ExecuteEvent();
                }
            }
        }
    }

    public void EventEnded()
    {
        curEvent.EventEnded();
        Destroy(curEvent.gameObject);
    }
}
