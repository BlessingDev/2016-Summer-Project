using UnityEngine;
using System.Collections;

public class OpenShopPopup : MonoBehaviour
{
    [SerializeField]
    private SkinType shopType = 0;

    public void OnClick()
    {
        ShopManager.Instance.OpenShopPopup(shopType);
    }
}
