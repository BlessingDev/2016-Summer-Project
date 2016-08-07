using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public struct ProblemData
{
    public string subject;
    public string summary;
    public bool answer;
}

public class LoadExam : Manager<LoadExam>
{
    [SerializeField]
    private GameObject preText;
    private Text text;
    private Text subject;

    [SerializeField]
    private GameObject preOMR;
    private Animator omr;

    [SerializeField]
    private GameObject preAnswerButtons;

    [SerializeField]
    private GameObject preProgressText;
    private Text progressText;

    private Dictionary<string, List<ProblemData>> problemDatas;
    private List<ProblemData> selectedProblems;

    private bool curAnswer;
    private int curShowIndex = 0;
    private const int PROBLEMS_PER_SUBJECT = 3;

    private int correctNum = 0;
    public int CorrectNum
    {
        get
        {
            return correctNum;
        }
    }
    private float scorePerProblem;

    private Date latestTestDate;
    public Date LatestTestDate
    {
        get
        {
            return latestTestDate;
        }
    }

    public int NumOfWholeProblems
    {
        get
        {
            return selectedProblems.Count;
        }
    }
    

    /*
    // public string[] values; // 문제 짝수 0 포함 정답 그다음 홀수
    // int ExamCount; // 문제 갯수

     string[] answer;
     string[] quest;
     

     public int num = 0; // 순서 및 번호 관리 안함
     */
    
    void Start()
    {
        Instance.Init();
        problemDatas = new Dictionary<string, List<ProblemData>>();
        selectedProblems = new List<ProblemData>();

        if(preOMR == null || preText == null ||
            preAnswerButtons == null || preProgressText == null)
        {
            Debug.LogError("Prefabs ARE NOT READY");
            enabled = false;
            return;
        }

        LoadTextFile("Exam");
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if(level == SceneManager.Instance.GetLevel("ExamScene"))
        {
            InitTestScene();
        }
    }

    void InitTestScene()
    {
        GameObject obj = Instantiate(preText);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        text = obj.transform.GetChild(0).GetComponent<Text>();
        subject = obj.transform.GetChild(1).GetComponent<Text>();

        obj = Instantiate(preAnswerButtons);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        obj = Instantiate(preOMR);
        obj.transform.SetParent(GameManager.Instance.World.transform);
        obj.transform.localPosition = new Vector2(472.7f, 0);
        obj.transform.localScale = Vector3.one;
        omr = obj.GetComponent<Animator>();

        obj = Instantiate(preProgressText);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = new Vector2(-510, -334);
        obj.transform.localScale = Vector3.one;
        progressText = obj.GetComponent<Text>();

        correctNum = 0;

        SelectProblems();
        ShowNextProblem();
    }

    void LoadTextFile(string fileName)
    {
        string t = "";
        string line = "";
        TextAsset asset = Resources.Load("Datas/Exam", typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(asset.text);
        if (sr == null)
        {
            Debug.LogError("Error : " + Application.dataPath + "/Resources/" + fileName);
            return;
        }
        else
        {
            line = sr.ReadLine();
            while (line != null)
            {
                int curIndex = 0;
                ProblemData data = new ProblemData();

                while (line[curIndex] != '<')
                    curIndex += 1;

                string val = ReadUntilTagEnd(line, curIndex + 1, out curIndex);
                data.subject = val;

                while (line[curIndex] != '<')
                    curIndex += 1;

                val = ReadUntilTagEnd(line, curIndex + 1, out curIndex);
                data.summary = val;

                while (line[curIndex] != '<')
                    curIndex += 1;

                val = ReadUntilTagEnd(line, curIndex + 1, out curIndex);
                data.answer = bool.Parse(val);

                List<ProblemData> datas;
                if(problemDatas.ContainsKey(data.subject))
                {
                    problemDatas.TryGetValue(data.subject, out datas);
                    datas.Add(data);

                    problemDatas.Remove(data.subject);
                    problemDatas.Add(data.subject, datas);
                }
                else
                {
                    datas = new List<ProblemData>();

                    datas.Add(data);
                    problemDatas.Add(data.subject, datas);
                }
                /*
                t += line;
                line = sr.ReadLine();
                if (line != null)
                {
                    //values = t.Split(','); 
                }
                */

                line = sr.ReadLine();
            }
            sr.Close();
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

    private void ShowNextProblem()
    {
        if(curShowIndex < selectedProblems.Count)
        {
            subject.text = selectedProblems[curShowIndex].subject;
            text.text = selectedProblems[curShowIndex].summary;
            curAnswer = selectedProblems[curShowIndex].answer;

            progressText.text = (curShowIndex + 1).ToString() + "/" + selectedProblems.Count.ToString();
            curShowIndex += 1;
        }
        else
        {
            TestEnded();
        }
    }

    private void SelectProblems()
    {
        if (selectedProblems.Count > 0)
            selectedProblems.Clear();
        
        foreach(var iter in problemDatas)
        {
            List<ProblemData> list = iter.Value;
            for(int i = 0; i < PROBLEMS_PER_SUBJECT; i += 1)
            {
                int selectedIdx = Random.Range(0, list.Count);

                selectedProblems.Add(list[selectedIdx]);
                list.RemoveAt(selectedIdx);
            }
        }

        scorePerProblem = 100f / selectedProblems.Count;
    }

    /*
    public string Getvalse(int numCount)
    {
        //return values[numCount];
    }
    

    public int GetExamCount()
    {
        return ExamCount;
    }
    /*
    public void ClickTrue()
    {
        if((num * 2) -1 > values.Length)
        {
            Debug.Log("모든 문제가  출력되었습니다");
            return;
        }
        if(Convert.ToBoolean(num))
        {
            Debug.Log("정답입니다");
        }
        else if (!Convert.ToBoolean(num))
        {
            Debug.Log("오답입니다");
        }
        num++;
        tx.text = values[num * 2];
    }

    public void ClickFalse()
    {
        if ((num * 2) -1 > values.Length)
        {
            Debug.Log("모든 문제가  출력되었습니다");
            return;
        }
        if (!Convert.ToBoolean(num))
        {
            Debug.Log("정답입니다");
        }
        else if (Convert.ToBoolean(num))
        {
            Debug.Log("오답입니다");
        }
        num++;
        tx.text = values[num * 2];
    }
     * */

    public void AnswerMarked(bool answer)
    {
        if(answer)
        {
            omr.SetBool("O", true);
        }
        else
        {
            omr.SetBool("X", true);
        }

        if(curAnswer == answer)
        {
            correctNum += 1;
        }

        UIManager.Instance.SetEnableTouchLayer("Main", false);
    }

    public void AnimationEnded()
    {
        omr.SetBool("O", false);
        omr.SetBool("X", false);

        UIManager.Instance.SetEnableTouchLayer("Main", true);
        ShowNextProblem();
    }

    public void TestEnded()
    {
        Date date = GameManager.Instance.GameDate;
        date.Day += 1;
        GameManager.Instance.GameDate = date;
        GameManager.Instance.LatestScore += (int)(correctNum * scorePerProblem);
        latestTestDate = GameManager.Instance.GameDate;

        GameManager.Instance.scheduleButtonType = ScheduleButtonType.Schedule;
        SceneManager.Instance.ChangeScene("GameScene");
    }
}