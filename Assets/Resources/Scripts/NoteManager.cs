using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public struct NoteData
{
    public string subjectName;
    public int limitLine;
    public string summary;
}

public class NoteManager :  Manager<NoteManager>
{
    private List<NoteData> noteDatas;
    [SerializeField]
    private GameObject preNoteText;
    private Text leftSubjectText;
    private Text leftText;
    private Text rightSubjectText;
    private Text rightText;
    [SerializeField]
    private GameObject preLeftButton;
    [SerializeField]
    private GameObject preRightButton;
    private Image leftButton;
    private Image rightButton;
    [SerializeField]
    private GameObject preNote;
    private Animator noteAnimator;
    private List<NoteData> possibleNoteDatas;
    private Dictionary<string, string> koreanSubjectNames;

    private int curIndex = 0;

	// Use this for initialization
	void Start ()
    {
        Instance.Init();


        if (preRightButton == null || preLeftButton == null ||
            preNoteText == null || preNote == null)
        {
            Debug.LogError("Prefabs are NOT READY");
            enabled = false;
            return;
        }

        koreanSubjectNames = new Dictionary<string, string>();
        koreanSubjectNames.Add("Korean", "국어");
        koreanSubjectNames.Add("Social", "사회");
        koreanSubjectNames.Add("Math", "수학");

        noteDatas = new List<NoteData>();

        ParseNoteFile();
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if(level == SceneManager.Instance.GetLevel("NoteScene"))
        {
            InitNoteScene();
        }
    }

    private void InitNoteScene()
    {
        GameObject obj = Instantiate(preNote);
        obj.transform.SetParent(GameManager.Instance.World.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        noteAnimator = obj.GetComponent<Animator>();

        obj = Instantiate(preNoteText);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = new Vector2(-212, 0);
        obj.transform.localScale = Vector3.one;
        leftSubjectText = obj.transform.GetChild(0).GetComponent<Text>();
        leftText = obj.transform.GetChild(1).GetComponent<Text>();

        obj = Instantiate(preNoteText);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = new Vector2(250, 0);
        obj.transform.localScale = Vector3.one;
        rightSubjectText = obj.transform.GetChild(0).GetComponent<Text>();
        rightText = obj.transform.GetChild(1).GetComponent<Text>();

        obj = Instantiate(preLeftButton);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = new Vector2(-362, -228.7f);
        obj.transform.localScale = Vector3.one;
        leftButton = obj.GetComponent<Image>();

        obj = Instantiate(preRightButton);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = new Vector2(370, -228.7f);
        obj.transform.localScale = Vector3.one;
        rightButton = obj.GetComponent<Image>();

        InitPossibleNodeData();
        RefreshText();
        CheckTurnOverEnable();
    }

    // 노트 신에 들어갈 때마다 불러서 보여줄 수 있는 노트들을 미리 넣어 놓는다.
    private void InitPossibleNodeData()
    {
        possibleNoteDatas = new List<NoteData>();

        foreach(var iter in noteDatas)
        {
            float val;
            GameManager.Instance.GetParameter(iter.subjectName, out val);
            if (val >= iter.limitLine)
            {
                possibleNoteDatas.Add(iter);
            }
        }
    }

    private void ParseNoteFile()
    {
        TextAsset asset = Resources.Load<TextAsset>("Datas/Note");
        string fileData = asset.text;

        int curIndex = 0;

        while (curIndex < fileData.Length)
        {
            while (fileData[curIndex] != '<')
                curIndex += 1;

            NoteData data = new NoteData();

            string val = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
            data.subjectName = val;

            if (val == "End")
                break;

            while (fileData[curIndex] != '<')
                curIndex += 1;

            val = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
            data.limitLine = int.Parse(val);

            while (fileData[curIndex] != '<')
                curIndex += 1;

            data.summary = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
            data.summary = data.summary.Replace("§", "");

            noteDatas.Add(data);
        }
    }

    private string ReadUntilTagEnd(string data, int startIndex, out int index)
    {
        string str = "";

        while (data[startIndex] != '>' || (data[startIndex] == '>' && data[startIndex - 1] == '§'))
        {
            str += data[startIndex];

            startIndex += 1;
        }

        index = startIndex;
        return str;
    }

    public void CheckTurnOverEnable()
    {
        if(curIndex <= 0)
        {
            leftButton.enabled = false;
        }

        if(curIndex + 2 >= possibleNoteDatas.Count)
        {
            rightButton.enabled = false;
        }
    }

    public void RefreshText()
    {
        for(int i = curIndex; i < Mathf.Min(curIndex + 2, possibleNoteDatas.Count); i += 1)
        {
            if(i == curIndex)
            {
                string subject;
                if(!koreanSubjectNames.TryGetValue(possibleNoteDatas[i].subjectName, out subject))
                {
                    Debug.LogError("Subject " + possibleNoteDatas[i].subjectName + " DOES NOT HAVE Korean Name");
                    return;
                }

                leftSubjectText.text = subject;
                leftText.text = possibleNoteDatas[i].summary;
            }
            else
            {
                string subject;
                if (!koreanSubjectNames.TryGetValue(possibleNoteDatas[i].subjectName, out subject))
                {
                    Debug.LogError("Subject " + possibleNoteDatas[i].subjectName + " DOES NOT HAVE Korean Name");
                    return;
                }

                rightSubjectText.text = subject;
                rightText.text = possibleNoteDatas[i].summary;
            }
        }
    }

    public void TurnOverLeft()
    {
        leftButton.enabled = false;
        rightButton.enabled = false;

        leftSubjectText.enabled = false;
        leftText.enabled = false;
        rightSubjectText.enabled = false;
        rightText.enabled = false;

        noteAnimator.SetBool("Left", true);
        curIndex -= 2;
    }

    public void TurnOverRight()
    {
        leftButton.enabled = false;
        rightButton.enabled = false;

        leftSubjectText.enabled = false;
        leftText.enabled = false;
        rightSubjectText.enabled = false;
        rightText.enabled = false;

        noteAnimator.SetBool("Right", true);
        curIndex += 2;
    }

    public void AnimationEnded()
    {
        leftSubjectText.enabled = true;
        leftText.enabled = true;
        rightSubjectText.enabled = true;
        rightText.enabled = true;

        leftButton.enabled = true;
        rightButton.enabled = true;

        RefreshText();
        CheckTurnOverEnable();
    }
}
