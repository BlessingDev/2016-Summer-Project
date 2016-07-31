using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SyncSteakerNum : MonoBehaviour
{
    [SerializeField]
    private Steaker steaker = null;

    private Text text = null;

	// Use this for initialization
	void Start ()
    {
        if(steaker == null)
        {
            Debug.LogError("steaker Is Null");
        }

        text = GetComponent<Text>();

        if(text == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Text");
            enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(steaker.Num >= 0)
        {
            text.text = "x" + steaker.Num.ToString();
        }
        else if (steaker.Num == -1)
        {
            text.text = "x∞";
        }
	}
}
