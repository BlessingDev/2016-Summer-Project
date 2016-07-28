using UnityEngine;
using System.Collections;

public class ButtonResetSchedule : MonoBehaviour
{
    public void OnClick()
    {
        SchedulingManager.Instance.ResetSchedules();
    }
}
