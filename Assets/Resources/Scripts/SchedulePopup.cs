using UnityEngine;
using System.Collections;

public class SchedulePopup : MonoBehaviour
{
    [SerializeField]
    GameObject parameters;
    [SerializeField]
    Animator animator;

    // Use this for initialization
    void Start()
    {
        if (parameters == null)
        {
            Debug.LogError("parameters not ready");
            enabled = false;
        }
        if (animator == null)
        {
            Debug.LogError("animator not ready");
            enabled = false;
        }
	}

    public void InitParameters()
    {
        for(int i = 0; i < parameters.transform.childCount; i += 1)
        {
            Transform obj = parameters.transform.GetChild(i);
            Destroy(obj.gameObject);
        }
    }

    public void AddParameter(GameObject obj)
    {
        obj.transform.SetParent(parameters.transform);
        obj.transform.localScale = new Vector3(0.7f, 0.7f, 1);
    }

    public void SetAnimationType(int type)
    {
        animator.SetInteger("AnimationType", type);
    }
}
