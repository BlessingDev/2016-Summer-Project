using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public struct ConversationData
{
    public int talkerCode;
    public string convSummary;
    public bool hasDistracter;
    public DistracterData distracter;
}

public struct DistracterData
{
    public int code;
    public string[] distracters;
    public string[] links;
}

public class ConversationManager : Manager<ConversationManager>
{
    private const string PARSER_VERSION = "1.0";
    private Dictionary<int, string> talkerName;
    private List<ConversationData> convDatas;
    private Dictionary<int, int> selectedDistracter;
    private int curConvIndex = 0;                // 현재 리스트의 어느 인덱스인지
    [SerializeField]
    private GameObject preConversationText = null;
    private ShowTextSlowly convText = null;
    [SerializeField]
    private GameObject preTalkerText = null;
    private Text talkerText = null;
    [SerializeField]
    private GameObject preDistracterPopup = null;
    private GameObject distracterPopup = null;
    [SerializeField]
    private GameObject preDistracterButton = null;
    private int curDistracterCode;
    private string[] curDistracterLinks;

	// Use this for initialization
	void Start ()
    {
        talkerName = new Dictionary<int, string>();
        convDatas = new List<ConversationData>();
        selectedDistracter = new Dictionary<int, int>();

        talkerName.Add(-1, "");
        talkerName.Add(0, "주인공");
        talkerName.Add(1, "면접관1");
        talkerName.Add(2, "면접관2");

        if(preConversationText == null || preTalkerText == null ||
            preDistracterPopup == null || preDistracterButton == null)
        {
            Debug.LogError("Prefabs are not READY");
            enabled = false;
            return;
        }

        convText = Instantiate(preConversationText).GetComponent<ShowTextSlowly>();
        convText.transform.SetParent(UIManager.Instance.Canvas.transform);
        convText.transform.localPosition = new Vector2(0, -222);
        convText.transform.localScale = Vector3.one;

        talkerText = Instantiate(preTalkerText).GetComponent<Text>();
        talkerText.transform.SetParent(UIManager.Instance.Canvas.transform);
        talkerText.transform.localPosition = new Vector2(-396, -116);
        talkerText.transform.localScale = Vector3.one;

        ParseConvFile("Interview_Basic");
        ShowText();
	}

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);
        
        if (level == SceneManager.Instance.GetLevel("ConversationScene"))
        {
            convText = Instantiate(preConversationText).GetComponent<ShowTextSlowly>();
            convText.transform.SetParent(UIManager.Instance.Canvas.transform);
            convText.transform.localPosition = new Vector2(0, -222);
            convText.transform.localScale = Vector3.one;

            talkerText = Instantiate(preTalkerText).GetComponent<Text>();
            talkerText.transform.SetParent(UIManager.Instance.Canvas.transform);
            talkerText.transform.localPosition = new Vector2(-396, -116);
            talkerText.transform.localScale = Vector3.one;

            ShowText();
        }
    }

    public void ParseConvFile(string fileName)
    {
        string fileData = FileManager.Instance.ReadFile(fileName + ".conv");
        
        int curIndex = 0;

        while (fileData[curIndex] != '<')
            curIndex += 1;
        string fileVer = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

        // 만약 파서의 버전과 대화 파일의 버전이 다르다면
        if(fileVer != PARSER_VERSION)
        {
            Debug.LogError("The file " + fileName + " has version " + fileVer + " but parser is version " + PARSER_VERSION);
            return;
        }

        while (curIndex < fileData.Length)
        {

            if (fileData[curIndex] == '<')
            {
                ConversationData convData = new ConversationData();

                string code = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
                convData.talkerCode = int.Parse(code);

                // 종료 코드 검사
                if(convData.talkerCode == -2)
                {
                    convDatas.Add(convData);

                    return;
                }

                while (fileData[curIndex] != '<')
                    curIndex += 1;

                string summary = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
                convData.convSummary = summary;

                while (fileData[curIndex] != '<')
                    curIndex += 1;

                // 선택지 여부
                string distracter = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
                convData.hasDistracter = bool.Parse(distracter);
                
                // 선택지 파싱
                if(convData.hasDistracter)
                {
                    DistracterData data = ParseDistracters(fileData, curIndex, out curIndex);
                    convData.distracter = data;
                }
                convDatas.Add(convData);
            }

            curIndex += 1;
        }
    }

    private string ReadUntilTagEnd(string data, int startIndex, out int index)
    {
        string str = "";
        
        while (data[startIndex] != '>')
        {
            str += data[startIndex];

            startIndex += 1;
        }

        index = startIndex;
        return str;
    }

    private DistracterData ParseDistracters(string data, int startIndex, out int index)
    {
        while (data[startIndex] != '<')
            startIndex += 1;

        DistracterData disData = new DistracterData();
        string code = ReadUntilTagEnd(data, startIndex + 1, out startIndex);
        disData.code = int.Parse(code);

        while (data[startIndex] != '<')
            startIndex += 1;

        string numStr = ReadUntilTagEnd(data, startIndex + 1, out startIndex);
        int num = int.Parse(numStr);

        disData.distracters = new string[num];
        disData.links = new string[num];

        for(int i = 0; i < num; i += 1)
        {
            while (data[startIndex] != '<')
                startIndex += 1;

            string summary = ReadUntilTagEnd(data, startIndex + 1, out startIndex);

            disData.distracters[i] = summary;

            while (data[startIndex] != '<')
                startIndex += 1;

            string link = ReadUntilTagEnd(data, startIndex + 1, out startIndex);

            disData.links[i] = link;
        }

        index = startIndex;
        return disData;
    }

    public void StartConversationEvent(string eventName)
    {
        SceneManager.Instance.ChangeScene("ConversationScene");
        ParseConvFile(eventName + "_Basic");
        ShowText();
    }

    //다음 텍스트를 보이는 것과 관련된 가공을 맡는다.
    public void ShowText()
    {
        if(curConvIndex < convDatas.Count)
        {
            if (convText.setNewString(convDatas[curConvIndex].convSummary))
            {
                string talker;
                talkerName.TryGetValue(convDatas[curConvIndex].talkerCode, out talker);
                talkerText.text = talker;

                if(convDatas[curConvIndex].hasDistracter)
                {
                    StartCoroutine(CorCheckDistracter(curConvIndex));
                }

                curConvIndex += 1;
            }
            else
            {
                convText.ShowWholeText();
            }
        }
        else if (curConvIndex == convDatas.Count)
        {
            convText.ShowWholeText();
        }
    }

    private IEnumerator CorCheckDistracter(int index)
    {
        while(true)
        {
            if(convText.CanBeUsed)
            {
                ShowDistracter(convDatas[index].distracter);
                StopCoroutine(CorCheckDistracter(index));
                break;
            }

            yield return null;
        }
    }

    private void ShowDistracter(DistracterData disData)
    {
        UIManager.Instance.SetEnableTouchLayer("Main", false);

        distracterPopup = Instantiate(preDistracterPopup);
        distracterPopup.transform.SetParent(UIManager.Instance.Canvas.transform);
        distracterPopup.transform.localPosition = Vector3.zero;
        distracterPopup.transform.localScale = Vector3.one;
        curDistracterCode = disData.code;
        curDistracterLinks = disData.links;

        for(int i = 0; i < disData.distracters.Length; i += 1)
        {
            DistracterSlot slot = Instantiate(preDistracterButton).GetComponent<DistracterSlot>();
            slot.distracterNum = i;
            slot.Text.text = disData.distracters[i];
            slot.transform.SetParent(distracterPopup.transform);
            slot.transform.localScale = Vector3.one;
            slot.Text.supportRichText = true;
        }

        GridLayoutGroup group = distracterPopup.GetComponent<GridLayoutGroup>();
        int height = (720 - 100 * disData.distracters.Length) / (disData.distracters.Length + 1);

        group.padding.top = height;
        group.spacing = new Vector2(0, height);
    }

    public void DistracterSelected(int distracterNum)
    {
        Debug.Log("Distracter " + distracterNum + " selected");
        UIManager.Instance.SetEnableTouchLayer("Main", true);
        selectedDistracter.Add(curDistracterCode, distracterNum);
        Destroy(distracterPopup);
        distracterPopup = null;

        InitConversationDatas();
        ParseConvFile(curDistracterLinks[distracterNum]);
        ShowText();
    }

    public void InitConversationDatas()
    {
        convDatas.Clear();
        curConvIndex = 0;
    }
}
