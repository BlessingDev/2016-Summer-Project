using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    private string spriteName;
    public string SpriteName
    {
        get
        {
            return spriteName;
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

    private bool inited = false;

    public void GetSpriteName()
    {
        inited = true;
        if (transform.childCount == 0)
        {
            Debug.LogError("This Slot DOESN'T HAVE Child");
            enabled = false;
            return;
        }

        var sprite = transform.GetChild(0).GetComponent<Image>();
        if (sprite == null)
        {
            Debug.LogError("This Slot DOESN'T HAVE Sprite Child");
            enabled = false;
            return;
        }

        spriteName = sprite.sprite.name;
    }

    void Start()
    {
        if(!inited)
        {
            GetSpriteName();
        }
    }

    public void OnClick()
    {
        ShopManager.Instance.SelectedSkinName = spriteName;
    }
}
