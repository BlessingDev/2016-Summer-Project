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

        GameManager.Instance.GetParameter(parameterName);

        Update();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (text != null)
            text.text = GameManager.Instance.GetParameter(parameterName).ToString();
        else if (cusText != null)
            cusText.Text = GameManager.Instance.GetParameter(parameterName).ToString();
    }
}
