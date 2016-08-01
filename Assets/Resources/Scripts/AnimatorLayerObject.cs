using UnityEngine;
using System.Collections;

public class AnimatorLayerObject : MonoBehaviour {

    [SerializeField]
    private string layerName = "";

	// Use this for initialization
	void Start ()
    {
        Animator animator = GetComponent<Animator>();

        if(animator == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Animator");
            return;
        }

        GameManager.Instance.AddAnimator(layerName, animator);
	}
}
