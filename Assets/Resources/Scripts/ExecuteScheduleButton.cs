using UnityEngine;
using System.Collections;

public class ExecuteScheduleButton : MonoBehaviour {
    public void OnClick()
    {
        GameManager.Instance.ScheduleExecute();
    }
}
