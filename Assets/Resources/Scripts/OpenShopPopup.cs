using UnityEngine;
using System.Collections;

public class OpenShopPopup : MonoBehaviour
{
    [SerializeField]
    private int shopType = 0;

    public void OnClick()
    {
        ShopManager.Instance.OpenShopPopup(shopType);
    }
}
