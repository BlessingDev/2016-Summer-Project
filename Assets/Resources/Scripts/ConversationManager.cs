using UnityEngine;
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

public class ConversationFileDataAlign : ConversationFileDataBase
{
    public string align = "";
}

public class ConversationFileDataPrefab : ConversationFileDataBase
{
    public string prefabName;
}

public class ConversationFileDataFile : ConversationFileDataBase
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
    private HashSet<string> compatitableVersion;

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

    private Dictionary<string, GameObject> preLoadedPrefabs;

    // Use this for initialization
    void Start()
    {
        Instance.Init();
        talkerName = new Dictionary<int, string>();
        convDatas = new List<ConversationFileDataBase>();
        selectedDistracter = new Dictionary<int, int>();
        preLoadedSprites = new Dictionary<string, Sprite>();
        parameters = new Dictionary<string, int>();
        compatitableVersion = new HashSet<string>();

        talkerName.Add(-1, "");
        talkerName.Add(0, "주인공");
        talkerName.Add(1, "면접관 1");
        talkerName.Add(2, "면접관 2");
        talkerName.Add(3, "선생님");

        compatitableVersion.Add("2.0");
        compatitableVersion.Add("2.1");

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

            parameters = new Dictionary<string, int>();

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
        if(asset == null)
        {
            Debug.LogError("Path " + curEventBasicPath + fileName + " DOES NOT FOUND");
            return;
        }
        preLoadedPrefabs = new Dictionary<string, GameObject>();
        preLoadedSprites = new Dictionary<string, Sprite>();

        string fileData = asset.text;

        int curIndex = 0;

        while (fileData[curIndex] != '<')
            curIndex += 1;
        string fileVer = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

        // 만약 파서의 버전과 대화 파일의 버전이 다르다면
        if (!compatitableVersion.Contains(fileVer))
        {
            Debug.LogError("The file " + fileName + " has version " + fileVer + " is NOT COMPATITABLE");
            return;
        }

        while (curIndex < fileData.Length)
        {
            if (fileData[curIndex] == '<')
            {
                int startIndex = curIndex;
                string code = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                ParseCodes(code, ref fileData, startIndex, curIndex, out curIndex);
            }

            curIndex += 1;
        }
    }
    
    private void ParseCodes(string code, ref string fileData, int startIndex, int curIndex, out int index)
    {

        switch (code)
        {
            case "Conversation":
                ParseConversation(fileData, curIndex + 1, out curIndex);

                break;
            case "Background":
                ParseBackground(fileData, curIndex + 1, out curIndex);

                break;
            case "Parameter":
                ParseParameters(ref fileData, startIndex, curIndex + 1, out curIndex);
                break;
            case "End":
                ConversationFileDataBase data = new ConversationFileDataBase();
                data.firstCode = "End";

                convDatas.Add(data);
                break;
            case "Align":
                ConversationFileDataAlign aData = new ConversationFileDataAlign();
                aData.firstCode = "Align";
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                aData.align = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                convDatas.Add(aData);
                break;
            case "Prefab":
                ConversationFileDataPrefab pData = new ConversationFileDataPrefab();
                pData.firstCode = "Prefab";
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                pData.prefabName = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                GameObject obj = Resources.Load<GameObject>("Prefabs/Conversations/Events/" + pData.prefabName);
                preLoadedPrefabs.Add(pData.prefabName, obj);

                convDatas.Add(pData);

                break;
            case "If":
                ParseCondition(ref fileData, curIndex, out curIndex);

                break;
            case "File":
                while (fileData[curIndex] != '<')
                    curIndex += 1;

                string link = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                ConversationFileDataFile fData = new ConversationFileDataFile();
                fData.firstCode = "File";
                fData.fileName = link;

                convDatas.Add(fData);
                break;
            default:
                Debug.LogError("CAN'T PARSE TAG " + code);

                break;
        }

        index = curIndex;
    }

    private void ParseCondition(ref string fileData, int curIndex, out int index)
    {
        while (fileData[curIndex] != '<')
            curIndex += 1;

        string ope = "";
        int startIndex = curIndex;
        int[] num = new int[2];
        int idx = 0;

        curIndex += 1;
        while (fileData[curIndex] != '>')
        {
            int curStartIndex = curIndex;
            
            while (fileData[curIndex] != '<')
                curIndex += 1;

            string code = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

            switch(code)
            {
                case "Parameter":
                    ParseParameters(ref fileData, curStartIndex, curIndex + 1, out curIndex);
                    break;
                case "==":
                case "!=":
                case "<=":
                case ">=":
                case "<":
                case ">":
                    ope = code;

                    break;
                default:
                    bool chk = false;
                    for(int i = 0; i < code.Length; i += 1)
                    {
                        if(!char.IsDigit(code[i]))
                        {
                            chk = true;
                            break;
                        }
                    }

                    if(!chk)
                    {
                        num[idx++] = int.Parse(code);
                    }

                    break;
            }

            curIndex += 1;
        }

        bool chk2 = false; ;
        switch(ope)
        {
            case "==":
                chk2 = num[0] == num[1];
                break;
            case "!=":
                chk2 = num[0] != num[1];
                break;
            case "<=":
                chk2 = num[0] <= num[1];
                break;
            case ">=":
                chk2 = num[0] >= num[1];
                break;
            case "<":
                chk2 = num[0] < num[1];
                break;
            case ">":
                chk2 = num[0] > num[1];
                break;
        }


        while (fileData[curIndex] != '<')
            curIndex += 1;

        curIndex += 1;
        while (curIndex < fileData.Length && fileData[curIndex] != '>')
        {
            int curStartIndex = curIndex;

            while (fileData[curIndex] != '<')
                curIndex += 1;

            startIndex = curIndex;
            string code = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

            if (chk2)
                ParseCodes(code, ref fileData, startIndex, curIndex + 1, out curIndex);

            curIndex += 1;
        }


        index = curIndex;
    }

    private void ParseParameters(ref string fileData, int startIndex, int curIndex, out int index)
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
            case "Get":
                while (fileData[curIndex] != '<')
                    curIndex += 1;

                parameterName = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                if (!parameters.ContainsKey(parameterName))
                {
                    Debug.LogError("Parameter " + parameterName + " is NOT DIMMED Or IMPORTED");
                    break;
                }
                else
                {
                    parameters.TryGetValue(parameterName, out val);

                    string replaceDes = new string(fileData.ToCharArray(), startIndex, curIndex - startIndex + 1);

                    int oriLen = fileData.Length;
                    fileData = fileData.Replace(replaceDes, "<" + val.ToString() + ">");
                    curIndex -= curIndex - startIndex + 1;

                }

                break;
            case "Import":
                while (fileData[curIndex] != '<')
                    curIndex += 1;

                parameterName = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                val = (int)GameManager.Instance.GetParameter(parameterName);
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

    /*private void ParseConvv1(string fileData, int curIndex)
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
    }*/

    private string ReadUntilTagEnd(string data, int startIndex, out int index)
    {
        string str = "";

        while (data[startIndex] != '>' || (data[startIndex] == '>' && data[startIndex - 1] == '§'))
        {
            str += data[startIndex];

            startIndex += 1;
        }

        str = str.Replace("§", "");
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

            startIndex += 1;
            while (data[startIndex] != '>')
            {
                while (data[startIndex] != '<')
                    startIndex += 1;

                while (data[startIndex + 1] == '<')
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

    private void ParseAlignData()
    {
        ConversationFileDataAlign align =
            (ConversationFileDataAlign)convDatas[curConvIndex];
        
        switch(align.align)
        {
            case "CenterCenter":
                convText.Text.alignment = TextAnchor.MiddleCenter;

                break;
        }
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

            if(convText.CanBeUsed)
            {
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
                        case "Align":
                            ParseAlignData();

                            break;
                        case "Prefab":
                            ParsePrefab((ConversationFileDataPrefab)convDatas[curConvIndex]);

                            break;
                        case "File":
                            string link = ((ConversationFileDataFile)convDatas[curConvIndex]).fileName;

                            ParseConvFile(link);

                            InitConversationDatas();
                            ParseConvFile(link);
                            return;
                        case "End":
                            EventManager.Instance.EventEnded();
                            return;
                        default:
                            Debug.LogError("CAN'T Parse Tag " + code);

                            break;
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
        
        curIndex += 1;
        while (curIndex < result.Length && result[curIndex] != '>')
        {
            int startIndex = curIndex;
            string code = ReadUntilTagEnd(result, curIndex + 1, out curIndex);
            
            ParseCodes(code, ref result, startIndex, curIndex + 1, out curIndex);
            
            curIndex += 1;
        }

        ShowText();
    }

    private void ParsePrefab(ConversationFileDataPrefab data)
    {
        GameObject obj;
        if(preLoadedPrefabs.TryGetValue(data.prefabName, out obj))
        {
            GameObject pre = Instantiate(obj);
            pre.transform.SetParent(UIManager.Instance.Canvas.transform);
            pre.transform.localPosition = obj.transform.localPosition;
            pre.transform.localScale = Vector3.one;

            Transform parent = convText.transform.parent;
            parent.SetSiblingIndex(parent.GetSiblingIndex() + 1);

            parent = talkerText.transform.parent;
            parent.SetSiblingIndex(parent.GetSiblingIndex() + 1);

            parent = distracterPopup.transform;
            parent.SetSiblingIndex(parent.GetSiblingIndex() + 1);
        }
        else
        {
            Debug.LogError("Prefab " + data.prefabName + " Is NOT LOADED");
        }
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

    public int GetParameter(string parameterName)
    {
        int val = -1;
        if(parameters.TryGetValue(parameterName, out val))
        {
            return val;
        }
        else
        {
            return val;
        }
    }
}
