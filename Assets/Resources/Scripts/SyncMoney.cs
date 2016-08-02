using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SyncMoney : MonoBehaviour
{
    private CustomNumberText cusText = null;
    private Text text = null;

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        cusText = GetComponent<CustomNumberText>();

        if (text == null && cusText == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Textable Component");
            enabled = false;
            return;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (text != null)
            text.text = GameManager.Instance.Money.ToString();
        else if (cusText != null)
            cusText.Text = GameManager.Instance.Money.ToString();
    }
}
