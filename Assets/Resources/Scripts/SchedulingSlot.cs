using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SchedulingSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private int time = 0;

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(item)
        {
            Destroy(item);
        }
        SchedulingDragHandler.draggingItem.transform.SetParent(transform);
        SchedulingDragHandler.draggingItem.transform.localScale = Vector3.one;
        SchedulingManager.Instance.SetSchedule(time, SchedulingDragHandler.draggingItem.GetComponent<SchedulingDragHandler>().Type);
    }
}
