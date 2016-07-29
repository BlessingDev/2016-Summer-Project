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

    void Start()
    {
        GameObject obj = SchedulingManager.Instance.GetSteaker(time);
        if (obj != null)
        {
            obj.transform.parent = transform;
            obj.transform.localScale = Vector3.one;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(SchedulingDragHandler.draggingItem == null)
        {
            Debug.LogWarning("draggingItem is NULL");
            return;
        }

        if (item)
        {
            Destroy(item);
        }

        SchedulingDragHandler.draggingItem.transform.SetParent(transform);
        SchedulingDragHandler.draggingItem.transform.localScale = Vector3.one;
        SchedulingManager.Instance.SetSchedule(time, SchedulingDragHandler.draggingItem.GetComponent<SchedulingDragHandler>().Type);
    }

    public void OnClick()
    {
        if(item)
        {
            Destroy(item);

            SchedulingManager.Instance.DeleteAt(time);

            item.GetComponent<SchedulingDragHandler>().OriHandler.Steaker.Num += 1;
        }
    }
}
