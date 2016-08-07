using UnityEngine;
using System.Collections.Generic;

public class EventManager : Manager<EventManager>
{
    GameEvent[] events;
    private GameEvent curEvent;
    private Dictionary<string, GameObject> preEvents;

    // Use this for initialization
    void Start ()
    {
        preEvents = new Dictionary<string, GameObject>();
        var objs = Resources.LoadAll<GameObject>("Prefabs/Events/");

        for(int i = 0; i < objs.Length; i += 1)
        {
            GameEvent eve = objs[i].GetComponent<GameEvent>();
            eve.Init();
            preEvents.Add(eve.EventName, objs[i]);
        }
	}
    
    public void InitEvents()
    {
        events = new GameEvent[preEvents.Count];
        int i = 0;
        foreach(var iter in preEvents)
        {
            events[i] = Instantiate(iter.Value).GetComponent<GameEvent>();
            i += 1;
        }
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
                    DontDestroyOnLoad(events[i].gameObject);
                    events[i].ExecuteEvent();
                }
            }

            if(curEvent != null)
            {
                if(curEvent.EndConditionCheck())
                {
                    EventEnded();
                }
            }
        }
    }

    public void SetCurEvent(string eventName)
    {
        GameObject obj;
        if(preEvents.TryGetValue(eventName, out obj))
        {
            obj = Instantiate(obj);
            DontDestroyOnLoad(obj);
            curEvent = obj.GetComponent<GameEvent>();
        }
    }

    public void EventEnded()
    {
        if(curEvent != null)
        {
            curEvent.EventEnded();
            Destroy(curEvent.gameObject);
            curEvent = null;
        }
        else
        {
            Debug.LogWarning("there are NOT ANY Events");
        }
    }
}
