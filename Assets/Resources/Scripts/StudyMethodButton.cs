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
            Sprite sprite = Resources.Load<Sprite>("Sprites/PausePopup/UI_Button_Sound_On_Pushed");
            GetComponent<Image>().sprite = sprite;
        }
	}
	
    public void OnClick()
    {
        SchedulingManager.Instance.StudyMode = method;
        SchedulingManager.Instance.CloseStudyMethodPopup();
    }
}
