using UnityEngine;
using System.Collections;

public class CostumeSlot : MonoBehaviour {

    [SerializeField]
    private int costumeCode = 0;
    public int CostumeCode
    {
        get
        {
            return costumeCode;
        }
    }

    [SerializeField]
    private int price = 0;
	public int Price
    {
        get
        {
            return price;
        }
    }

    public void OnClick()
    {
        ShopManager.Instance.SelectedSkinName = costumeCode.ToString();
    }
}
