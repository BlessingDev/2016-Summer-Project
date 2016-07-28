using UnityEngine;
using System.Collections;
using System;

public class EscObserverScheduleScene : EscObserver
{
    public override void EscAction()
    {
        SceneManager.Instance.ChangeScene("GameScene");
    }
}
