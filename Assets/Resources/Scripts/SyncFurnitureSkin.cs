using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SyncFurnitureSkin : MonoBehaviour {

    [SerializeField]
    private SkinType furnitureType = 0;
    private SpriteRenderer sprite = null;
    private Image image = null;

	// Use this for initialization
	void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        if(sprite == null && image == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Sprite Component");
            enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (sprite != null)
            sprite.sprite = GameManager.Instance.GetAppropriateSkin(furnitureType);
        else if (image != null)
            image.sprite = GameManager.Instance.GetAppropriateSkin(furnitureType);
    }
}
