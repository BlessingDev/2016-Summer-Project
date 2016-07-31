using UnityEngine;
using System.Collections;
using System;

public class GameEventTest : GameEvent
{
    public override bool ConditionCheck()
    {
        Date date = GameManager.Instance.GameDate;

        if (date.Month == 5 && date.Day == 3)
            return true;
        else if (date.Month == 6 && date.Day == 28)
            return true;
        else
            return false;
    }

    public override void ExecuteEvent()
    {
        Debug.LogError("Test Tomorrow");
    }
}
