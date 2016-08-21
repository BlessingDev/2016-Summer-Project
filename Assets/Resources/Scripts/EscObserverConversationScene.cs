using UnityEngine;
using System.Collections;
using System;

public class EscObserverConversationScene : EscObserver {

    public override void EscAction()
    {
        SceneManager.Instance.ChangeScene("GameScene");
    }
}
