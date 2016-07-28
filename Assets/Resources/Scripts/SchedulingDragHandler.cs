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
    public SchedulingDragHandler OriHandler
    {
        get
        {
            return oriHandler;
        }
    }
    private Steaker steaker;
    public Steaker Steaker
    {
        get
        {
            return steaker;
        }
    }

    [SerializeField]
    ScheduleType type;
    public ScheduleType Type
    {
        get
        {
            return type;
        }
    }

    void Start()
    {
        steaker = GetComponent<Steaker>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(steaker.Num > 0 || steaker.Num == -1)
        {
            startParent = UIManager.Instance.Canvas.transform;
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            moveObj = Instantiate<GameObject>(gameObject);
            moveObj.transform.parent = startParent;
            moveObj.transform.localScale = Vector3.one;
            draggingItem = moveObj;
            moveObj.GetComponent<SchedulingDragHandler>().oriHandler = this;
        }
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
        else
        {
            if(steaker.Num > 0)
            {
                steaker.Num -= 1;
            }
        }
        moveObj = null;
    }
}
