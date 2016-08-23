using UnityEngine;
using System.Collections;

public class ReserveDestroy : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private float sec = 1f;

	// Use this for initialization
	void Start ()
    {
	    if(obj != null)
        {
            StartCoroutine(reserve());
        }
        else
        {
            Debug.LogWarning("There is NO obj");
        }
	}

    private IEnumerator reserve()
    {
        yield return new WaitForSeconds(sec);

        Destroy(obj);
    }
}
