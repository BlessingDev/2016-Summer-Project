using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarControl : MonoBehaviour {

    [SerializeField]
    private Mask mMask = null;
    [SerializeField]
    private int mOriginalValue = 100;
    [SerializeField]
    private int mCurVal = 100;

    private bool mIsMoving = false;         // 현재 코루틴을 통한 바 갱신이 진행 중인가?
    private float mSpeed = 0.3f;             // 바가 갱신되는 스피드

	// Use this for initialization
	void Start ()
    {
        if (mMask == null)
            this.enabled = false;
	}
	
    public void SetValue(int val)
    {
        if(!mIsMoving)
            StartCoroutine(UpdateBar(mCurVal, val));
    }

    private IEnumerator UpdateBar(int from, int to)
    {
        mIsMoving = true;
        float val = mSpeed;

        if (from > to)
            val *= -1;

        float targetScale = (float)to / mOriginalValue;

        while(true)
        {
            Vector3 scale = mMask.transform.localScale;

            bool endCheck = (val < 0) ? (scale.x <= targetScale) : (scale.x >= targetScale);
            if (endCheck)
            {
                mIsMoving = false;
                StopAllCoroutines();
                break;
            }

            scale.x += val * Time.smoothDeltaTime;

            mMask.transform.localScale = scale;

            yield return null;
        }
    }
}
