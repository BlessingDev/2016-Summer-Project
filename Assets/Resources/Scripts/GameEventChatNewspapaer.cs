using UnityEngine;
using System.Collections;
using System;

public class GameEventChatNewspapaer : GameEvent {

    static bool executed = false;

    public override void Init()
    {
        eventName = "Chat Newspaper";
    }

    public override bool ConditionCheck()
    {
        if(!executed)
        {
            if (!GameManager.Instance.isVacation &&
                SchedulingManager.Instance.GameTime >= 9 &&
                SchedulingManager.Instance.GameTime <= 18)
            {
                float possi = UnityEngine.Random.Range(0f, 100f);
                if(possi <= 0.001f)
                {
                    executed = true;
                    return true;
                }
            }
        }

        return false;
    }

    public override void ExecuteEvent()
    {
        GameManager.Instance.SetParameter("SchoolNewspaperPolicy", 1);
        ConversationManager.Instance.StartConversationEvent("Chat Newspaper");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        GameManager.Instance.ScheduleExecute();
    }
}
