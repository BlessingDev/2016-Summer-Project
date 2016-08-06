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
            var handler = obj.GetComponent<SchedulingDragHandler>();
            GameObject oriHandler;
            SchedulingManager.Instance.CurSteakerDic.TryGetValue(handler.Type, out oriHandler);
            handler.OriHandler = oriHandler.GetComponent<SchedulingDragHandler>();
            handler.OriHandler.GetComponent<Steaker>().Num -= 1;
            Destroy(obj.transform.GetChild(0).gameObject);
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
            if (SchedulingManager.Instance.DeleteAt(time))
            {
                Destroy(item);
                var handler = item.GetComponent<SchedulingDragHandler>();
                handler.OriHandler.Steaker.Num += 1;

                SchedulingDragHandler.draggingItem.transform.SetParent(transform);
                SchedulingDragHandler.draggingItem.transform.localScale = Vector3.one;
                SchedulingManager.Instance.SetSchedule(time, SchedulingDragHandler.draggingItem.GetComponent<SchedulingDragHandler>().Type);
            }
        }
        else
        {
            SchedulingDragHandler.draggingItem.transform.SetParent(transform);
            SchedulingDragHandler.draggingItem.transform.localScale = Vector3.one;
            SchedulingManager.Instance.SetSchedule(time, SchedulingDragHandler.draggingItem.GetComponent<SchedulingDragHandler>().Type);
        }
    }

    public void OnClick()
    {
        if(item)
        {
            if(SchedulingManager.Instance.DeleteAt(time))
            {
                Destroy(item);
                var handler = item.GetComponent<SchedulingDragHandler>();
                handler.OriHandler.Steaker.Num += 1;
            }
        }
    }
}
