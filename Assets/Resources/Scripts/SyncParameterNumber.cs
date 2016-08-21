using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SyncParameterNumber : MonoBehaviour
{
    private Text text = null;
    private CustomNumberText cusText = null;
    [SerializeField]
    private string parameterName = "";

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        cusText = GetComponent<CustomNumberText>();

        if(text == null && cusText == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Textable Component");
            enabled = false;
            return;
        }

        Update();
	}
	
	// Update is called once per frame
	void Update ()
    {
        float val;
        GameManager.Instance.GetParameter(parameterName, out val);
        if (text != null)
            text.text = val.ToString();
        else if (cusText != null)
            cusText.Text = val.ToString();
    }
}
