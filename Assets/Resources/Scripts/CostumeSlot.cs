using UnityEngine;
using System.Collections;

public class CostumeSlot : MonoBehaviour {

    [SerializeField]
    private int costumeCode = 0;
	
    public void OnClick()
    {
        ShopManager.Instance.SelectedSkinName = costumeCode.ToString();
    }
}
