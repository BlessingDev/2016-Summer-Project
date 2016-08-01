using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SyncParameterNumber : MonoBehaviour
{
    private Text text = null;
    [SerializeField]
    private string parameterName = "";

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();

        if(text == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Text");
            enabled = false;
            return;
        }

        GameManager.Instance.GetParameter(parameterName);
	}
	
	// Update is called once per frame
	void Update ()
    {
        text.text = GameManager.Instance.GetParameter(parameterName).ToString();
	}
}
