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

    // Use this for initialization
    void Start ()
    {
        canvas = FindObjectOfType<Canvas>();
	}
	
	// Update is called once per frame
	void update ()
    {

	}
}
