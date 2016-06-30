using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager mInstance = null;
    public static GameManager Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = FindObjectOfType<GameManager>();

            if (mInstance == null)
                mInstance = new GameObject("GameManager", new System.Type[] { typeof(GameManager) }).GetComponent<GameManager>();

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

    private bool mPause = false;
    public bool IsPause
    {
        get
        {
            return mPause;
        }
    }
    private float mTimeRate = 0;
    private float mTime = 0;

    private float mHunger = 100f;
    public float Hunger
    {
        get
        {
            return mHunger;
        }
    }
    private float mHungerDecresePerTime = 15f;
    private float mBefUpdateTime = 0;

	// Use this for initialization
	void Start ()
    {
        mInited = true;
        mTimeRate = 24f / 180f; // 1일은 3분
	}

    void Update()
    {
        if (!mPause)
        {
            TimeUpdate();
            StatusUpdate();
        }
    }

    private void TimeUpdate()
    {
        mTime += Time.smoothDeltaTime * mTimeRate;
    }

    private void StatusUpdate()
    {
        if(mTime - mBefUpdateTime >= 0.16f) // 10분이 지나면
        {
            mHunger -= mHungerDecresePerTime / 6;
            mBefUpdateTime = mTime;
        }
    }
}
