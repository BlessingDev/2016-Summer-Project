using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StudyMethodButton : MonoBehaviour
{
    [SerializeField]
    private int method = 0;

	// Use this for initialization
	void Start ()
    {
	    if(method == SchedulingManager.Instance.StudyMode)
        {
            Sprite sprite = Resources.Load<Sprite>("Sprites/ScheduleUI/Study_" + name + "_Checked");
            GetComponent<Image>().sprite = sprite;
        }
	}

    public void SetToNormalSprite()
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/ScheduleUI/Study_" + name);
        GetComponent<Image>().sprite = sprite;
    }

    public void OnDown()
    {
        var objs = FindObjectsOfType<StudyMethodButton>();

        for (int i = 0; i < 3; i += 1)
        {
            if(objs[i] != this)
            {
                objs[i].SetToNormalSprite();
            }
        }
    }

    public void OnClick()
    {
        SchedulingManager.Instance.StudyMode = method;
        SchedulingManager.Instance.CloseStudyMethodPopup();
    }
}
