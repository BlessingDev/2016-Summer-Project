using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ScheduleDirectionButton : MonoBehaviour
{

    [SerializeField]
    private int direction;
	
    public void OnClick()
    {
        SchedulingManager.Instance.MoveScheduleList(direction);
    }
}
