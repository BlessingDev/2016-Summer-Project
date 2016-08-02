using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    [SerializeField]
    private Button purchaseButton = null;
    private Image purchaseImage = null;
    private Sprite purchaseImageNormal = null;
    private Sprite purchaseImagePushed = null;

    [SerializeField]
    private Button useButton = null;
    private Image useImage = null;
    private Sprite useImageNormal = null;
    private Sprite useImagePushed = null;

    void Start()
    {
        if(purchaseButton == null || useButton == null)
        {
            Debug.LogError("purchaseButton or useButton is NULL");
            enabled = false;
            return;
        }

        purchaseImage = purchaseButton.GetComponent<Image>();
        purchaseImageNormal = purchaseImage.sprite;
        purchaseImagePushed = purchaseButton.spriteState.pressedSprite;

        useImage = useButton.GetComponent<Image>();
        useImageNormal = useImage.sprite;
        useImagePushed = useButton.spriteState.pressedSprite;
    }

    public void NotBought()
    {
        purchaseButton.enabled = true;
        useButton.enabled = false;

        purchaseImage.sprite = purchaseImageNormal;
        useImage.sprite = useImagePushed;
    }

    public void Bought()
    {
        useButton.enabled = true;
        purchaseButton.enabled = false;

        purchaseImage.sprite = purchaseImagePushed;
        useImage.sprite = useImageNormal;
    }
}
