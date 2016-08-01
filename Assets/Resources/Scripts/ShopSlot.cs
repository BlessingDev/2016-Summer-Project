using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private string spriteName;

	// Use this for initialization
	void Start ()
    {
	    if(transform.childCount == 0)
        {
            Debug.LogError("This Slot DOESN'T HAVE Child");
            enabled = false;
            return;
        }

        var sprite = transform.GetChild(0).GetComponent<Image>();
        if(sprite == null)
        {
            Debug.LogError("This Slot DOESN'T HAVE Sprite Child");
            enabled = false;
            return;
        }

        spriteName = sprite.sprite.name;
	}

    public void OnClick()
    {
        ShopManager.Instance.SelectedSkinName = spriteName;
    }
}
