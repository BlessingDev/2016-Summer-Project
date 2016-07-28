using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SchedulingDragHandler : MonoBehaviour
    , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggingItem;
    Transform startParent;
    GameObject moveObj;
    SchedulingDragHandler oriHandler;

    [SerializeField]
    ScheduleType type;
    public ScheduleType Type
    {
        get
        {
            return type;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = UIManager.Instance.Canvas.transform;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

        moveObj = Instantiate<GameObject>(gameObject);
        moveObj.transform.parent = startParent;
        moveObj.transform.localScale = Vector3.one;
        draggingItem = moveObj;
        oriHandler = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        moveObj.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (moveObj.transform.parent == startParent)
        {
            Destroy(moveObj);
        }
        moveObj = null;
    }
}
