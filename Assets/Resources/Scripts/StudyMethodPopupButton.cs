using UnityEngine;
using System.Collections;

public class StudyMethodPopupButton : MonoBehaviour {
    public void OnClick()
    {
        SchedulingManager.Instance.OpenStudyMethodPopup();
    }
}
