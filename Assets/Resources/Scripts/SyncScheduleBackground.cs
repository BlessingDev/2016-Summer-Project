using UnityEngine;
using System.Collections;

public class SyncScheduleBackground : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        string name = GameManager.Instance.CurSkinNames[(int)SkinType.Desk - 1];
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Sprite sprite = null;

        switch(name)
        {
            case "Map_Desk":
            case "Map_Desk_3":
                sprite = Resources.Load<Sprite>("Sprites/ScheduleUI/Background/schedule_background_1");

                break;
            case "더나은테이블":
                sprite = Resources.Load<Sprite>("Sprites/ScheduleUI/Background/schedule_background_2");

                break;
        }

        if(sprite != null)
            renderer.sprite = sprite;
        else
        {
            Debug.LogError("COULDN'T FIND Sprite for " + name);
        }
    }
}
