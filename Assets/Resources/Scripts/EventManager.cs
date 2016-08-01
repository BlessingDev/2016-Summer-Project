﻿using UnityEngine;
using System.Collections;

public class EventManager : Manager<EventManager>
{
    GameEvent[] events;

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
                    events[i].ExecuteEvent();
                }
            }
        }
    }
}