using UnityEngine;
using System.Collections;
using System;

public class EscObserverMainScene : EscObserver {

    public override void EscAction()
    {
        Application.Quit();
    }
}
