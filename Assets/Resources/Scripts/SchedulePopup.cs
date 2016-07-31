using UnityEngine;
using System.Collections;

public class SchedulePopup : MonoBehaviour
{
    [SerializeField]
    GameObject parameters;

	// Use this for initialization
	void Start ()
    {
	    if(parameters == null)
        {
            Debug.LogError("parameters not ready");
            enabled = false;
        }
	}

    public void InitParameters()
    {
        for(int i = 0; i < parameters.transform.childCount; i += 1)
        {
            var obj = parameters.transform.GetChild(i);
            Destroy(obj);
        }
    }

    public void AddParameter(GameObject obj)
    {
        obj.transform.SetParent(parameters.transform);
        obj.transform.localScale = Vector3.one;
    }
}
