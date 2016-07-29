using UnityEngine;
using System.Collections;

public class ShopPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject scrolledRect = null;
    public GameObject ScrolledRect
    {
        get
        {
            return scrolledRect;
        }
    }
}
