using UnityEngine;
using System.Collections;

public class PurchaseButton : MonoBehaviour {

    public void OnClick()
    {
        ShopManager.Instance.Use();
    }
}
