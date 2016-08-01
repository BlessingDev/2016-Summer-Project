using UnityEngine;
using System.Collections;

public class UseButton : MonoBehaviour {

    public void OnClick()
    {
        ShopManager.Instance.Use();
    }
}
