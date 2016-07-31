using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarControl : MonoBehaviour {

    [SerializeField]
    private Transform scaleTransform = null;
    [SerializeField]
    private int mOriginalValue = 100;
    public int OriginalValue
    {
        get
        {
            return mOriginalValue;
        }
        set
        {
            mOriginalValue = value;
        }
    }
    [SerializeField]
    private float mCurVal = 100;
    [SerializeField]
    private Transform barTransform = null;
    [SerializeField]
    private bool vertical = true;

    private bool mIsMoving = false;         // 현재 코루틴을 통한 바 갱신이 진행 중인가?
    private float mSpeed = 0.3f;             // 바가 갱신되는 스피드

	// Use this for initialization
	void Start ()
    {
        if (scaleTransform == null)
        {
            enabled = false;
            Debug.LogError("scaleTransform is NULL");
        }
    }
	
    public void SetValue(float val)
    {
        if(!mIsMoving)
            StartCoroutine(UpdateBar(mCurVal, val));
    }

    public void SetValueImmediately(float val)
    {
        if (val > mOriginalValue)
            val = mOriginalValue;
        else if (val < 0)
            val = 0;

        float targetScale = val / mOriginalValue;
        Vector3 scale = scaleTransform.transform.localScale;
        if(vertical)
        {
            scale.y = targetScale;
        }
        else
        {
            scale.x = targetScale;
        }
        scaleTransform.transform.localScale = scale;

        if(barTransform != null)
        {
            float resciprocalScale = mOriginalValue / val;
            scale = barTransform.localScale;
            if (vertical)
            {
                scale.y = targetScale;
            }
            else
            {
                scale.x = targetScale;
            }
            barTransform.localScale = scale;
        }
    }

    private IEnumerator UpdateBar(float from, float to)
    {
        mIsMoving = true;
        float val = mSpeed;

        if (from > to)
            val *= -1;

        float targetScale = (float)to / mOriginalValue;

        while(true)
        {
            Vector3 scale = scaleTransform.localScale;

            float curVal = 0f;
            if (vertical)
                curVal = scale.y;
            else
                curVal = scale.x;

            bool endCheck = (val < 0) ? (curVal <= targetScale) : (curVal >= targetScale);
            if (endCheck)
            {
                mIsMoving = false;
                StopAllCoroutines();
                break;
            }

            if (vertical)
                scale.y += val * Time.smoothDeltaTime;
            else
                scale.x += val * Time.smoothDeltaTime;

            scaleTransform.localScale = scale;

            yield return null;
        }
    }
}
