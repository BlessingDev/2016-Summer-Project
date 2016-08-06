using UnityEngine;
using System.Collections;

public class SyncValtScrollZOrder : MonoBehaviour {

    private float height;
    private SpriteRenderer renderer;

	// Use this for initialization
	void Start ()
    {
        renderer = GetComponent<SpriteRenderer>();
        if(renderer == null)
        {
            Debug.LogError("This Object DOESN'T HAVE SpriteRenderer");
            enabled = false;
            return;
        }

        height = renderer.sprite.rect.height;
	}
	
	// Update is called once per frame
	void Update ()
    {
        renderer.sortingOrder = 720 - (int)(transform.localPosition.y - (float)height / 2);
	}
}
