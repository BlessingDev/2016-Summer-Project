using UnityEngine;
using System.Collections;

public class CloseShopPopup : MonoBehaviour
{
    public void OnClick()
    {
        ShopManager.Instance.ClosePopup();
    }
}
