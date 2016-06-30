using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager mInstance = null;
    public static UIManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = FindObjectOfType<UIManager>();

            if (mInstance == null)
                mInstance = new GameObject("GameManager", new System.Type[] { typeof(UIManager) }).GetComponent<UIManager>();

            return mInstance;
        }
    }
    private static bool mInited = false;
    public static bool IsInited
    {
        get
        {
            return mInited;
        }
    }

    [SerializeField]
    private BarControl mHunger = null;

    // Use this for initialization
    void Start ()
    {
        if (mHunger == null)
        {
            this.enabled = false;
            return;
        }

        mInited = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateBar();
	}

    void UpdateBar()
    {
        mHunger.SetValue((int)GameManager.Instance.Hunger);
    }
}
