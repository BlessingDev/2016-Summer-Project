using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : Manager<UIManager>
{
    private Canvas canvas;
    public Canvas Canvas
    {
        get
        {
            if(curIndex != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
            {
                curIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
                canvas = FindObjectOfType<Canvas>();

                observer = FindObjectOfType<EscObserver>();
                if (layersDic != null)
                    layersDic.Clear();
            }

            return canvas;
        }
    }

    private EscObserver observer = null;
    private Dictionary<string, List<Image>> layersDic;

    private int curIndex = -1;

    public override void Init()
    {
        Start();

        base.Init();
    }

    // Use this for initialization
    void Start ()
    {
        if(!IsInited)
        {
            canvas = FindObjectOfType<Canvas>();
            observer = FindObjectOfType<EscObserver>();
            layersDic = new Dictionary<string, List<Image>>();
        }
	}
	
    override public void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if (curIndex != UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            curIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            canvas = FindObjectOfType<Canvas>();

            observer = FindObjectOfType<EscObserver>();
            if (layersDic != null)
                layersDic.Clear();
        }

    }

    // Update is called once per frame
    public void update ()
    {
        if (observer != null)
            observer.update();
	}

    public void AddObjectOnLayer(string layerName, Image obj)
    {
        if(layersDic.ContainsKey(layerName))
        {
            List<Image> list;
            layersDic.TryGetValue(layerName, out list);
            list.Add(obj);
        }
        else
        {
            List<Image> list = new List<Image>();
            list.Add(obj);
            layersDic.Add(layerName, list);
        }
    }

    public bool SetEnableTouchLayer(string layerName, bool ena)
    {
        if(layersDic.ContainsKey(layerName))
        {
            List<Image> list;
            layersDic.TryGetValue(layerName, out list);
            foreach(var iter in list)
            {
                iter.raycastTarget = ena;
            }

            return true;
        }
        else
        {
            Debug.LogWarning("INVALID LayerName");
            return false;
        }
    }
}
