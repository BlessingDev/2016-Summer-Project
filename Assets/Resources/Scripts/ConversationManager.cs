﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConversationFileDataBase
{
    public string firstCode;
}

public class ConversationFileDataConv : ConversationFileDataBase
{
    public int talkerCode;
    public string convSummary;
    public bool hasDistracter;
    public DistracterData distracter;

}

public class ConversationFileDataBackground : ConversationFileDataBase
{
    public string fileName;
}

public struct DistracterData
{
    public int code;
    public string[] distracters;
    public string[] results;
}

public class ConversationManager : Manager<ConversationManager>
{
    private const string PARSER_VERSION = "2.0";
    private Dictionary<int, string> talkerName;
    private List<ConversationFileDataBase> convDatas;
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
    private string[] curDistracterResults;
    private string curEventBasicPath;

    private Dictionary<string, int> parameters;
    private Dictionary<string, Sprite> preLoadedSprites;
    private SpriteRenderer background = null;
    public Dictionary<string, int> Parameters
    {
        get
        {
            return parameters;
        }
    }

    // Use this for initialization
    void Start()
    {
        Instance.Init();
        talkerName = new Dictionary<int, string>();
        convDatas = new List<ConversationFileDataBase>();
        selectedDistracter = new Dictionary<int, int>();
        preLoadedSprites = new Dictionary<string, Sprite>();
        parameters = new Dictionary<string, int>();

        talkerName.Add(-1, "");
        talkerName.Add(0, "주인공");
        talkerName.Add(1, "면접관 1");
        talkerName.Add(2, "면접관 2");

        if (preConversationText == null || preTalkerText == null ||
            preDistracterPopup == null || preDistracterButton == null)
        {
            Debug.LogError("Prefabs are not READY");
            enabled = false;
            return;
        }
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if (level == SceneManager.Instance.GetLevel("ConversationScene"))
        {
            UIManager.Instance.OnLevelWasLoaded(level);

            GameObject obj = Instantiate(preConversationText);
            obj.transform.SetParent(UIManager.Instance.Canvas.transform);
            obj.transform.localPosition = new Vector2(0, -222);
            obj.transform.localScale = Vector3.one;
            convText = obj.transform.GetChild(0).GetComponent<ShowTextSlowly>();

            obj = Instantiate(preTalkerText);
            obj.transform.SetParent(UIManager.Instance.Canvas.transform);
            obj.transform.localPosition = new Vector2(-465, -44);
            obj.transform.localScale = Vector3.one;
            talkerText = obj.transform.GetChild(0).GetComponent<Text>();

            background = GameObject.Find("Background").GetComponent<SpriteRenderer>();

            ShowText();
        }
    }

    private void ParseConvFile(string fileName)
    {
        TextAsset asset = Resources.Load<TextAsset>(curEventBasicPath + fileName);
        string fileData = asset.text;

        int curIndex = 0;

        while (fileData[curIndex] != '<')
            curIndex += 1;
        string fileVer = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

        // 만약 파서의 버전과 대화 파일의 버전이 다르다면
        if (fileVer != PARSER_VERSION)
        {
            Debug.LogError("The file " + fileName + " has version " + fileVer + " but parser is version " + PARSER_VERSION);
            return;
        }

        while (curIndex < fileData.Length)
        {
            if (fileData[curIndex] == '<')
            {
                string code = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                switch (code)
                {
                    case "Conversation":
                        ParseConversation(fileData, curIndex + 1, out curIndex);

                        break;
                    case "Background":
                        ParseBackground(fileData, curIndex + 1, out curIndex);

                        break;
                    case "Parameter":
                        ParseParameters(fileData, curIndex + 1, out curIndex);
                        break;
                    case "End":
                        ConversationFileDataBase data = new ConversationFileDataBase();
                        data.firstCode = "End";

                        convDatas.Add(data);
                        break;
                }
            }

            curIndex += 1;
        }
    }

    private void ParseParameters(string fileData, int curIndex, out int index)
    {
        while (fileData[curIndex] != '<')
            curIndex += 1;

        string order = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

        switch (order)
        {
            case "Dim":
                while (fileData[curIndex] != '<')
                    curIndex += 1;

                string parameterName = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                while (fileData[curIndex] != '<')
                    curIndex += 1;

                string valStr = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
                int val = int.Parse(valStr);

                parameters.Add(parameterName, val);

                break;
            case "Add":
                while (fileData[curIndex] != '<')
                    curIndex += 1;

                parameterName = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                if (!parameters.ContainsKey(parameterName))
                {
                    Debug.LogError("Parameter " + parameterName + " is NOT DIMMED");
                    break;
                }

                while (fileData[curIndex] != '<')
                    curIndex += 1;

                valStr = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
                val = int.Parse(valStr);

                int curVal = 0;

                parameters.TryGetValue(parameterName, out curVal);
                curVal += val;

                parameters.Remove(parameterName);
                parameters.Add(parameterName, curVal);

                break;
            case "Set":
                while (fileData[curIndex] != '<')
                    curIndex += 1;

                parameterName = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                if (!parameters.ContainsKey(parameterName))
                {
                    Debug.LogError("Parameter " + parameterName + " is NOT DIMMED");
                    break;
                }

                while (fileData[curIndex] != '<')
                    curIndex += 1;

                valStr = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
                val = int.Parse(valStr);

                parameters.Remove(parameterName);
                parameters.Add(parameterName, val);

                break;
        }

        index = curIndex;
    }

    private void ParseBackground(string fileData, int curIndex, out int index)
    {
        while (fileData[curIndex] != '<')
            curIndex += 1;

        string fileName = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

        Sprite sprite = Resources.Load<Sprite>("Sprites/Conversations/Backgrounds/" + fileName);
        preLoadedSprites.Add(fileName, sprite);

        ConversationFileDataBackground data = new ConversationFileDataBackground();
        data.fileName = fileName;
        data.firstCode = "Background";

        convDatas.Add(data);

        index = curIndex;
    }

    private void ParseConversation(string fileData, int curIndex, out int outIndex)
    {
        while (fileData[curIndex] != '<')
            curIndex += 1;
        ConversationFileDataConv convData = new ConversationFileDataConv();
        convData.firstCode = "Conversation";

        string code = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
        convData.talkerCode = int.Parse(code);

        // 종료 코드 검사
        if (convData.talkerCode == -2)
        {
            convDatas.Add(convData);

            outIndex = curIndex;
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
        if (convData.hasDistracter)
        {
            DistracterData data = ParseDistracters(fileData, curIndex, out curIndex);
            convData.distracter = data;
        }
        convDatas.Add(convData);

        outIndex = curIndex;

    }

    private void ParseConvv1(string fileData, int curIndex)
    {
        while (curIndex < fileData.Length)
        {

            if (fileData[curIndex] == '<')
            {
                ConversationFileDataConv convData = new ConversationFileDataConv();

                string code = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);
                convData.talkerCode = int.Parse(code);

                // 종료 코드 검사
                if (convData.talkerCode == -2)
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
                if (convData.hasDistracter)
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
        disData.results = new string[num];

        for (int i = 0; i < num; i += 1)
        {
            while (data[startIndex] != '<')
                startIndex += 1;

            string summary = ReadUntilTagEnd(data, startIndex + 1, out startIndex);

            disData.distracters[i] = summary;

            while (data[startIndex] != '<')
                startIndex += 1;

            while (data[startIndex] != '>')
            {
                while (data[startIndex] != '<')
                    startIndex += 1;

                string link = ReadUntilTagEnd(data, startIndex + 1, out startIndex);

                disData.results[i] += "<" + link + "> ";

                startIndex += 1;
            }
            disData.results[i] += ">";
        }


        index = startIndex;
        return disData;
    }

    public void StartConversationEvent(string eventName)
    {
        InitConversationDatas();
        SetCurEventBasicPath(eventName);
        ParseConvFile(eventName + "_Basic");
        SceneManager.Instance.ChangeScene("ConversationScene");
    }

    //다음 텍스트를 보이는 것과 관련된 가공을 맡는다.
    public void ShowText()
    {
        if (curConvIndex < convDatas.Count)
        {
            string code = convDatas[curConvIndex].firstCode;

            while (code != "Conversation" && curConvIndex < convDatas.Count)
            {
                switch (code)
                {
                    case "Background":
                        ConversationFileDataBackground data = (ConversationFileDataBackground)convDatas[curConvIndex];
                        Sprite sprite;
                        preLoadedSprites.TryGetValue(data.fileName, out sprite);

                        background.sprite = sprite;
                        break;
                    case "End":
                        EventManager.Instance.EventEnded();
                        return;
                }

                curConvIndex += 1;
                code = convDatas[curConvIndex].firstCode;
            }

            ConversationFileDataConv convData = (ConversationFileDataConv)convDatas[curConvIndex];
            if (convText.setNewString(convData.convSummary))
            {
                string talker;
                talkerName.TryGetValue(convData.talkerCode, out talker);
                talkerText.text = talker;

                if (convData.hasDistracter)
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
        while (true)
        {
            if (convText.CanBeUsed)
            {
                ShowDistracter(((ConversationFileDataConv)convDatas[index]).distracter);
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
        curDistracterResults = disData.results;

        for (int i = 0; i < disData.distracters.Length; i += 1)
        {
            DistracterSlot slot = Instantiate(preDistracterButton).GetComponent<DistracterSlot>();
            slot.distracterNum = i;
            slot.Text.text = disData.distracters[i];
            slot.transform.SetParent(distracterPopup.transform);
            slot.transform.localScale = Vector3.one;
            slot.Text.supportRichText = true;
        }

        GridLayoutGroup group = distracterPopup.GetComponent<GridLayoutGroup>();
        int height = (720 - (int)group.cellSize.y * disData.distracters.Length) / (disData.distracters.Length + 1);

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

        ParseDistracterResult(curDistracterResults[distracterNum]);
    }

    private void ParseDistracterResult(string result)
    {
        int curIndex = 0;
        while (result[curIndex] != '<')
            curIndex += 1;

        string link = "";
        while (curIndex < result.Length - 1 && result[curIndex + 1] != '>')
        {
            curIndex += 1;
            string code = ReadUntilTagEnd(result, curIndex + 1, out curIndex);

            switch (code)
            {
                case "Parameter":
                    ParseParameters(result, curIndex + 1, out curIndex);

                    break;
                case "File":
                    while (result[curIndex] != '<')
                        curIndex += 1;

                    link = ReadUntilTagEnd(result, curIndex + 1, out curIndex);

                    break;
            }
            curIndex += 1;
        }

        InitConversationDatas();
        ParseConvFile(link);
        ShowText();
    }

    private void SetCurEventBasicPath(string eventName)
    {
        curEventBasicPath = "Datas/Conversations/" + eventName + "/";
    }

    public void InitConversationDatas()
    {
        convDatas.Clear();
        curConvIndex = 0;
    }
}