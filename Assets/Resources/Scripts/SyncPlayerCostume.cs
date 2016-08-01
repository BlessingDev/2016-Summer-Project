using UnityEngine;
using System.Collections;

public class SyncPlayerCostume : MonoBehaviour {
    private Animator animator;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();

        if(animator == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Animator");
            enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        animator.SetInteger("CostumeCode", GameManager.Instance.CostumeCode);
	}
}
