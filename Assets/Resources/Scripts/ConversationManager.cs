using UnityEngine;
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
    private int curConvIndex = 0;                // 현재 리스트의 어느 인덱스인지
    [SerializeField]
    private Text convText = null;
    [SerializeField]
    private Text talkerText = null;

	// Use this for initialization
	void Start ()
    {
        talkerName = new Dictionary<int, string>();
        convDatas = new List<ConversationData>();

        talkerName.Add(-1, "");
        talkerName.Add(0, "주인공");
        talkerName.Add(1, "면접관1");
        talkerName.Add(2, "면접관2");

        if(convText == null || talkerText == null)
        {
            Debug.LogError("Text Components are not READY");
            enabled = false;
            return;
        }

        ParseConvFile("Interview_Basic");
        ShowText();
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
            convText.text = convDatas[curConvIndex].convSummary;
            string talker;
            talkerName.TryGetValue(convDatas[curConvIndex].talkerCode, out talker);
            talkerText.text = talker;

            curConvIndex += 1;
        }
    }
}
