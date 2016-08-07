using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * 이 스크립트는 주어진 문자열을 Text에 정해진 간격으로 천천히 띄워주는 역할을 한다. (미연시 사양 개조)
*/
public class ShowTextSlowly : MonoBehaviour
{
    private Text text = null;
    public Text Text
    {
        get
        {
            return text;
        }
    }
    private bool canBeUsed = true;      // 현재 사용가능한가?
    public bool CanBeUsed
    {
        get
        {
            return canBeUsed;
        }
    }
    public float secPerLetter = 0.05f; // 글자당 초 수

    private string mStrToShow = "NewLabel"; // 보여줄 전체 문자열

    void Start()
    {
        text = GetComponent<Text>();

        if(text == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Text Component");
            enabled = false;
            return;
        }
    }

    public bool setNewString(string fStr)
    {
        if (canBeUsed)
        {
            canBeUsed = false;
            mStrToShow = fStr;
            StartCoroutine(UpdateShowingString());
            return true;
        }
        else
        {
            Debug.LogWarning("ShowTextSlowly at " + name + " Can't be Used.");
            return false;
        }
    }

    public void ShowWholeText()
    {
        StopAllCoroutines();
        canBeUsed = true;
        text.text = mStrToShow;
    }

    public void ClearText()
    {
        text.text = "";
    }

    IEnumerator UpdateShowingString()
    {
        int curIdx = 0;
        float curTime = 0f;

        while (true)
        {
            if (canBeUsed)
                break;

            //만약 모든 문자를 표시했다면
            if (curIdx >= mStrToShow.Length)
            {
                canBeUsed = true;
                StopCoroutine(UpdateShowingString());
                break;
            }
            else
            {
                if (curTime >= secPerLetter)
                {
                    curIdx += 1;
                    // 모든 공백은 띄어넘는다
                    while (curIdx < mStrToShow.Length && mStrToShow[curIdx] == ' ')
                        curIdx += 1;

                    curTime -= secPerLetter;

                    string outputStr = new string(mStrToShow.ToCharArray(), 0, curIdx);

                    text.text = outputStr;
                }
                else
                {
                    curTime += Time.deltaTime;
                }
            }

            yield return null;
        }
    }
}