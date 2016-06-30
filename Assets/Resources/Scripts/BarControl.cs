using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarControl : MonoBehaviour {

    [SerializeField]
    private Image mRenderer = null;
    [SerializeField]
    private int mOriginalValue = 100;

    private float mOriginalWidth = 0;

	// Use this for initialization
	void Start ()
    {
        if (mRenderer == null)
            this.enabled = false;
        else
        {
            mOriginalWidth = mRenderer.sprite.rect.width;
        }
	}
	
    void SetValue(int val)
    {
        Vector3 scale = mRenderer.transform.localScale;

        scale.x = val / mOriginalValue;

        

        mRenderer.transform.localScale = scale;
    }
}
