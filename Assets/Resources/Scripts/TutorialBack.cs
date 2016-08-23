using UnityEngine;
using System.Collections;

public class TutorialBack : MonoBehaviour {

    [SerializeField]
    private GameObject obj;

	// Use this for initialization
	void Start ()
    {
	
	}
	
    public void OnClick()
    {
        Destroy(obj);
    }
}
