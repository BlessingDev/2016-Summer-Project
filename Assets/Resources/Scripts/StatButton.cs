using UnityEngine;
using System.Collections;

public class StatButton : MonoBehaviour
{
    private bool opened = false;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
    public void OnClick()
    {
        if(!opened)
        {
            GameManager.Instance.OpenStatPopup();
        }
        else
        {
            GameManager.Instance.CloseStatPopup();
        }
        opened = !opened;
    }
}
