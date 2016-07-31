using UnityEngine;
using System.Collections;

public class SyncParameter : MonoBehaviour
{
    private BarControl bar = null;
    [SerializeField]
    private string parameterName = "";

	// Use this for initialization
	void Start ()
    {
        bar = GetComponent<BarControl>();

        if(bar == null)
        {
            Debug.LogError("this object DOESN'T HAVE BarControl");
            enabled = true;
        }

        float check = GameManager.Instance.GetParameter(parameterName);

        bar.SetValueImmediately(GameManager.Instance.GetParameter(parameterName));
    }

    // Update is called once per frame
    void Update ()
    {
        bar.SetValueImmediately(GameManager.Instance.GetParameter(parameterName));
	}
}
