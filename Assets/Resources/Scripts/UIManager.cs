﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
    private Dictionary<string, List<Image>> layersDic;

    // Use this for initialization
    void Start ()
    {
        canvas = FindObjectOfType<Canvas>();
        observer = FindObjectOfType<EscObserver>();
        layersDic = new Dictionary<string, List<Image>>();
	}
	
    public void OnLevelWasLoaded(int level)
    {
        canvas = FindObjectOfType<Canvas>();
        observer = FindObjectOfType<EscObserver>();
        layersDic.Clear();
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
