﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotCollector : MonoBehaviour
{
    private Image[] images = new Image[12];
    private SchedulingSlot[] slots = new SchedulingSlot[12];

	// Use this for initialization
	public void Start ()
    {
	    for(int i = 0; i < 12; i += 1)
        {
            GameObject obj = transform.GetChild(i).gameObject;

            images[i] = obj.GetComponent<Image>();
            slots[i] = obj.GetComponent<SchedulingSlot>();
        }
	}

    public void SetImagesEnable(bool ena)
    {
        for(int i = 0; i < 12; i += 1)
        {
            images[i].enabled = ena;
        }
    }

    public void Reset()
    {
        for(int i = 0; i < 12; i += 1)
        {
            slots[i].OnClick();
        }
    }
}
