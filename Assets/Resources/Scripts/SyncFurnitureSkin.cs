using UnityEngine;
using System.Collections;

public class SyncFurnitureSkin : MonoBehaviour {

    [SerializeField]
    private SkinType furnitureType = 0;
    private SpriteRenderer sprite = null;

	// Use this for initialization
	void Start ()
    {
        sprite = GetComponent<SpriteRenderer>();
        
        if(sprite == null)
        {
            Debug.LogError("This Object DOESN'T HAVE SpriteRenderer");
            enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
