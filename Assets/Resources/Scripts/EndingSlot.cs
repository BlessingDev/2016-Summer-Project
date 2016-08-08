using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingSlot : MonoBehaviour
{
    [SerializeField]
    private Text endingName;
    [SerializeField]
    private Text summary;
    [SerializeField]
    private Image image;


	// Use this for initialization
	void Start ()
    {
	    if(endingName == null || summary == null ||
            image == null)
        {
            Debug.LogError("Texts Are NOT READY");
            return;
        }

        if(!AlbumManager.Instance.IsEndingOpened(name))
        {
            endingName.enabled = false;
            summary.enabled = false;
            image.enabled = false;
        }
	}


}
