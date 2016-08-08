using UnityEngine;
using System.Collections;
using System;

public class GameEventEnding : GameEvent
{
    public override bool ConditionCheck()
    {
        return false;
    }

    public override void ExecuteEvent()
    {
        
    }

    public override void EventEnded()
    {
        AlbumManager.Instance.SetEnding(eventName, true);
        SceneManager.Instance.ChangeScene("MainScene");
    }
}
