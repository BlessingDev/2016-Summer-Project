using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : Manager<UIManager>
{
    private Canvas canvas;
    public Canvas Canvas
    {
        get
        {
            return canvas;
        }
    }

    private EscObserver observer = null;

    // Use this for initialization
    void Start ()
    {
        canvas = FindObjectOfType<Canvas>();
        observer = FindObjectOfType<EscObserver>();
	}
	
    public void OnLevelWasLoaded(int level)
    {
        canvas = FindObjectOfType<Canvas>();
        observer = FindObjectOfType<EscObserver>();
    }

    // Update is called once per frame
    public void update ()
    {
        if (observer != null)
            observer.update();
	}
}
