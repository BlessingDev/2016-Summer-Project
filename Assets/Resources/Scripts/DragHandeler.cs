using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandeler : MonoBehaviour ,IBeginDragHandler , IDragHandler , IEndDragHandler{

    public static GameObject itemBeginDragged;
    Vector3 startPosition;
    Transform srartParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeginDragged = gameObject;
        startPosition = transform.position;
        srartParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeginDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == srartParent)
        {
            transform.position = startPosition;
        }
    }
}
