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

            moveObj = Instantiate(gameObject);
            moveObj.transform.parent = startParent;
            moveObj.transform.localScale = Vector3.one;
            moveObj.GetComponent<UnityEngine.UI.Image>().SetNativeSize();
            Destroy(moveObj.transform.GetChild(0).gameObject);
            draggingItem = moveObj;
            moveObj.GetComponent<SchedulingDragHandler>().oriHandler = this;
            if(steaker.Num > 0)
                steaker.Num -= 1;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (steaker.Num > 0 || steaker.Num == -1)
        {
            moveObj.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (steaker.Num > 0 || steaker.Num == -1)
        {
            draggingItem = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (moveObj.transform.parent == startParent)
            {
                if(steaker.Num > 0)
                    steaker.Num += 1;
                Destroy(moveObj);
            }
            moveObj = null;
        }
    }
}
