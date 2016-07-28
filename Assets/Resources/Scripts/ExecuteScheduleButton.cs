using UnityEngine;
using System.Collections;

public class ExecuteScheduleButton : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
    public void OnClick()
    {
        GameManager.Instance.ScheduleExecute();
    }
}
