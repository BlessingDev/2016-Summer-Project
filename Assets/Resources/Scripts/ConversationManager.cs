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

public class ConversationFileDataStanding : ConversationFileDataBase
{
    public string name;
    public string order;
    public int xPos;
    public string spriteName;
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

    private int convDataAddIndex = -1;

    private Image talkerBack;

    private Dictionary<string, SpriteRenderer> standingCGs;
    private string eventName;

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
        standingCGs = new Dictionary<string, SpriteRenderer>();

        talkerName.Add(-1, "");
        talkerName.Add(0, "나");
        talkerName.Add(1, "면접관 1");
        talkerName.Add(2, "면접관 2");
        talkerName.Add(3, "선생님");
        talkerName.Add(4, "모두들");
        talkerName.Add(5, "친구1");
        talkerName.Add(6, "방송");
        talkerName.Add(7, "후보1");

        compatitableVersion.Add("2.0");
        compatitableVersion.Add("2.1");
        compatitableVersion.Add("2.23");

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
            talkerBack = obj.GetComponent<Image>();

            background = GameObject.Find("Background").GetComponent<SpriteRenderer>();

            ParseConvFile(eventName + "_Basic");
            ShowText();
        }
    }

    private void ParseConvFile(string fileName)
    {
        TextAsset asset = Resources.Load<TextAsset>(curEventBasicPath + fileName);
        if (asset == null)
        {
            Debug.LogError("Path " + curEventBasicPath + fileName + " DOES NOT FOUND");
            return;
        }
        Debug.Log(fileName + " File Was Opened");


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

        int openedBracket = 0;
        curIndex += 1;
        while (curIndex < fileData.Length)
        {
            if (fileData[curIndex] == '<')
            {
                while (fileData[curIndex] == '<')
                {
                    openedBracket += 1;
                    curIndex += 1;
                }

                int startIndex = curIndex;
                string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                ParseCodes(code, ref fileData, startIndex, ref curIndex, ref openedBracket);
            }

            if (curIndex < fileData.Length &&  fileData[curIndex] == '>')
            {
                while (curIndex < fileData.Length && fileData[curIndex] == '>')
                {
                    openedBracket -= 1;
                    curIndex += 1;
                }
            }

            curIndex += 1;
        }

        Debug.Log("opened Brackets is " + openedBracket);
    }

    private void ParseCodes(string code, ref string fileData, int startIndex, ref int curIndex, ref int openedBracket)
    {
        while (openedBracket > 0 && curIndex < fileData.Length && fileData[curIndex] == '>')
        {
            curIndex += 1;
            openedBracket -= 1;
        }

        switch (code)
        {
            case "Conversation":
                ParseConversation(ref fileData, curIndex, out curIndex, ref openedBracket);

                break;
            case "Background":
                ParseBackground(fileData, curIndex, out curIndex);

                break;
            case "Parameter":
                ParseParameters(ref fileData, startIndex, curIndex, out curIndex, ref openedBracket);
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
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                aData.align = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                while (fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

                convDatas.Add(aData);
                break;
            case "Prefab":
                ConversationFileDataPrefab pData = new ConversationFileDataPrefab();
                pData.firstCode = "Prefab";
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                pData.prefabName = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                while (fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

                GameObject obj = Resources.Load<GameObject>("Prefabs/Conversations/Events/" + pData.prefabName);
                preLoadedPrefabs.Add(pData.prefabName, obj);

                convDatas.Add(pData);

                break;
            case "If":
                ParseCondition(ref fileData, ref curIndex, ref openedBracket);

                break;
            case "Comp":
                ParseComp(ref fileData, ref curIndex, startIndex);
                //Debug.Log("str " + fileData[curIndex - 1] + fileData[curIndex] + fileData[curIndex + 1]);
                openedBracket -= 1;

                break;
            case "File":
                while (fileData[curIndex] != '<')
                    curIndex += 1;

                string link = ReadUntilTagEnd(fileData, curIndex + 1, out curIndex);

                ConversationFileDataFile fData = new ConversationFileDataFile();
                fData.firstCode = "File";
                fData.fileName = link;

                convDatas.Add(fData);

                curIndex += 1;
                break;
            case "Standing":
                ParseStandingCG(fileData, ref curIndex);

                curIndex += 1;
                break;
            default:
                Debug.LogWarning("CAN'T PARSE TAG " + code);

                break;
        }
    }

    private bool ParseCondition(ref string fileData, ref int curIndex, ref int openedBracket, bool execute = true)
    {
        // <IF'>' 부분에서 들어온다.
        while (fileData[curIndex] == '>')
        {
            curIndex += 1;
            openedBracket -= 1;
        }


        int startBucket = openedBracket;
        while (fileData[curIndex] != '<')
            curIndex += 1;
        while (fileData[curIndex] == '<')
        {
            curIndex += 1;
            openedBracket += 1;
        }

        int startIndex = curIndex;
        do
        {
            if (fileData[curIndex] == '>')
            {
                while (fileData[curIndex] == '>' && openedBracket > startBucket)
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }
            }
            else if (fileData[curIndex] == '<')
            {
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                int curStartIndex = curIndex;
                string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                ParseCodes(code, ref fileData, curStartIndex, ref curIndex, ref openedBracket);
            }
            else
            {
                int curStartIndex = curIndex;
                string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                ParseCodes(code, ref fileData, curStartIndex, ref curIndex, ref openedBracket);
            }

        } while (openedBracket > startBucket);





        curIndex = startIndex - 1;
        while (fileData[curIndex] == '<')
        {
            curIndex += 1;
            openedBracket += 1;
        }

        string chk = ReadUntilTagEnd(fileData, curIndex, out curIndex);
        bool chk2 = bool.Parse(chk);
        while (fileData[curIndex] == '>')
        {
            curIndex += 1;
            openedBracket -= 1;
        }

        do
        {
            while (fileData[curIndex] != '<')
                curIndex += 1;
            while (fileData[curIndex] == '<')
            {
                curIndex += 1;
                openedBracket += 1;
            }

            int curStartIndex = curIndex;
            string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);

            if(execute)
            {
                if (chk2)
                    ParseCodes(code, ref fileData, curStartIndex, ref curIndex, ref openedBracket);
            }
            else
            {
                curIndex = curStartIndex;
                return chk2;
            }


            while (openedBracket > startBucket && fileData[curIndex] == '>')
            {
                curIndex += 1;
                openedBracket -= 1;
            }

        } while (openedBracket > startBucket);

        return chk2;
    }

    private void ParseComp(ref string fileData, ref int curIndex, int startIndex)
    {
        // <Comp>' '에서 진입

        int[] num = new int[2];
        bool[] boolean = new bool[2];
        string ope = null;
        int idx = 0;
        string replaceStr;

        while (idx < 2)
        {
            while (fileData[curIndex] != '<')
                curIndex += 1;
            while (fileData[curIndex] == '<')
            {
                curIndex += 1;
            }

            int curStartIndex = curIndex;
            string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);

            switch (code)
            {
                case "Comp":
                    ParseComp(ref fileData, ref curIndex, curStartIndex);

                    break;
                case "Parameter":
                    int bracket = 0;
                    ParseParameters(ref fileData, curStartIndex, curIndex, out curIndex, ref bracket);

                    break;
                case "==":
                case "!=":
                case "<=":
                case ">=":
                case "<":
                case ">":
                case "||":
                case "&&":
                    ope = code;

                    break;
                case "TRUE":
                case "FALSE":
                    boolean[idx++] = bool.Parse(code);

                    break;
                case "Random":
                    while (fileData[curIndex] != '<')
                        curIndex += 1;
                    while (fileData[curIndex] == '<')
                    {
                        curIndex += 1;
                    }

                    code = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                    int start = int.Parse(code);

                    while (fileData[curIndex] != '<')
                        curIndex += 1;
                    while (fileData[curIndex] == '<')
                    {
                        curIndex += 1;
                    }

                    code = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                    int end = int.Parse(code);

                    int dif = curIndex - startIndex + 1;

                    replaceStr = new string(fileData.ToCharArray(), startIndex, dif);

                    fileData = fileData.Replace(replaceStr, "<" + UnityEngine.Random.Range(start, end) + ">");

                    curIndex = startIndex;

                    break;
                default:
                    for (int i = 0; i < code.Length; i += 1)
                    {
                        if (!char.IsDigit(code[i]))
                        {
                            Debug.LogError("Not A Comparable Val " + code);
                        }
                    }

                    num[idx++] = int.Parse(code);

                    break;
            }
        }

        bool chk2 = false;
        switch (ope)
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
            case "&&":
                chk2 = boolean[0] && boolean[1];
                break;
            case "||":
                chk2 = boolean[0] || boolean[1];
                break;
        }

        fileData = fileData.Remove(startIndex, curIndex - startIndex + 1);
        fileData = fileData.Insert(startIndex, "<" + chk2.ToString().ToUpper() + ">");
        curIndex -= curIndex - startIndex + 1;
    }

    private void ParseNumber(ref string fileData, ref int curIndex)
    {
        while (fileData[curIndex] != '<')
            curIndex += 1;
        while (fileData[curIndex] == '<')
        {
            curIndex += 1;
        }

        int startIndex = curIndex;
        string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);
        string replaceStr;

        switch (code)
        {
            case "Random":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }

                code = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                int start = int.Parse(code);

                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }

                code = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                int end = int.Parse(code);
                
                fileData = fileData.Remove(startIndex - 1, curIndex - startIndex + 2);
                fileData = fileData.Insert(startIndex - 1, "<" + UnityEngine.Random.Range(start, end) + ">");

                curIndex = startIndex;

                break;
            case "Parameter":
                int openedBracket = 0;
                ParseParameters(ref fileData, startIndex, curIndex, out curIndex, ref openedBracket);

                break;
            default:
                for (int i = 0; i < code.Length; i += 1)
                {
                    if (!char.IsDigit(code[i]) && code[i] != '-')
                    {
                        Debug.LogError("Not A Comparable Val " + code);
                    }
                }

                break;
        }

        curIndex = startIndex;
    }

    private void ParseParameters(ref string fileData, int startIndex, int curIndex, out int index, ref int openedBracket)
    {
        while (fileData[curIndex] != '<')
            curIndex += 1;
        while (fileData[curIndex] == '<')
        {
            curIndex += 1;
            openedBracket += 1;
        }

        string order = ReadUntilTagEnd(fileData, curIndex, out curIndex);
        float fVal;

        switch (order)
        {
            case "Dim":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                string parameterName = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                while (fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

                ParseNumber(ref fileData, ref curIndex);

                string valStr = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                int val = int.Parse(valStr);


                parameters.Add(parameterName, val);

                break;
            case "Add":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                parameterName = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                if (!parameters.ContainsKey(parameterName))
                {
                    Debug.LogError("Parameter " + parameterName + " is NOT DIMMED");
                    break;
                }

                while (fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

                ParseNumber(ref fileData, ref curIndex);

                valStr = ReadUntilTagEnd(fileData, curIndex, out curIndex);
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
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                parameterName = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                if (!parameters.ContainsKey(parameterName))
                {
                    Debug.LogError("Parameter " + parameterName + " is NOT DIMMED");
                    break;
                }

                while (fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

                ParseNumber(ref fileData, ref curIndex);

                valStr = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                val = int.Parse(valStr);

                parameters.Remove(parameterName);
                parameters.Add(parameterName, val);

                Debug.Log(parameterName + " parameter set " + val);

                while (curIndex < fileData.Length && fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

                break;
            case "Get":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                parameterName = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                if (!parameters.ContainsKey(parameterName))
                {
                    Debug.LogError("Parameter " + parameterName + " is NOT DIMMED Or IMPORTED");
                    break;
                }
                else
                {
                    parameters.TryGetValue(parameterName, out val);
                    Debug.Log("paraetmer " + parameterName + " get " + val);

                    fileData = fileData.Remove(startIndex - 1, curIndex - startIndex + 2);
                    fileData = fileData.Insert(startIndex - 1, "<" + val + ">");
                    curIndex -= curIndex - startIndex + 1;

                }

                while (fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

                break;
            case "Import":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                parameterName = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                    openedBracket += 1;
                }

                ParseNumber(ref fileData, ref curIndex);

                valStr = ReadUntilTagEnd(fileData, curIndex, out curIndex);
                val = int.Parse(valStr);

                if (GameManager.Instance.GetParameter(parameterName, out fVal))
                {
                    parameters.Add(parameterName, (int)fVal);
                }
                else
                {
                    parameters.Add(parameterName, val);
                }

                while (fileData[curIndex] == '>')
                {
                    curIndex += 1;
                    openedBracket -= 1;
                }

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

    private void ParseConversation(ref string fileData, int curIndex, out int outIndex, ref int openedBracket)
    {
        int startBracket = openedBracket;
        while (fileData[curIndex] != '<')
            curIndex += 1;
        while (fileData[curIndex] == '<')
        {
            openedBracket += 1;
            curIndex += 1;
        }

        ConversationFileDataConv convData = new ConversationFileDataConv();
        convData.firstCode = "Conversation";

        string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);
        try
        {
            convData.talkerCode = int.Parse(code);
        }
        catch
        {
            Debug.LogError("talker Code is Wrong");
        }

        while (fileData[curIndex] == '>')
        {
            openedBracket -= 1;
            curIndex += 1;
        }


        while (fileData[curIndex] != '<')
            curIndex += 1;
        while (fileData[curIndex] == '<')
        {
            openedBracket += 1;
            curIndex += 1;
        }

        string summary = ReadUntilTagEnd(fileData, curIndex, out curIndex);
        convData.convSummary = summary;
        while (fileData[curIndex] == '>')
        {
            openedBracket -= 1;
            curIndex += 1;
        }

        while (fileData[curIndex] != '<')
            curIndex += 1;
        while (fileData[curIndex] == '<')
        {
            openedBracket += 1;
            curIndex += 1;
        }

        // 선택지 여부
        string distracter = ReadUntilTagEnd(fileData, curIndex, out curIndex);
        convData.hasDistracter = bool.Parse(distracter);

        while (fileData[curIndex] == '>')
        {
            openedBracket -= 1;
            curIndex += 1;
        }

        // 선택지 파싱
        if (convData.hasDistracter)
        {
            DistracterData data = ParseDistracters(ref fileData, curIndex, out curIndex, ref openedBracket);
            convData.distracter = data;
        }
        convDatas.Add(convData);


        outIndex = curIndex;
    }

    private void ParseStandingCG(string fileData, ref int curIndex)
    {
        while (fileData[curIndex] != '<')
            curIndex += 1;
        while (fileData[curIndex] == '<')
        {
            curIndex += 1;
        }

        string code = ReadUntilTagEnd(fileData, curIndex, out curIndex);
        ConversationFileDataStanding data = new ConversationFileDataStanding();
        data.firstCode = "Standing";
        string val;
        Sprite sprite = null;

        switch (code)
        {
            case "Set":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }
                val = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                data.name = val;

                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }
                val = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                data.xPos = int.Parse(val);

                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }
                val = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                data.spriteName = val;

                data.order = "Set";

                convDatas.Add(data);

                

                break;
            case "Change":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }
                val = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                data.name = val;

                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }
                val = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                data.spriteName = val;

                sprite = Resources.Load<Sprite>("Sprites/Conversations/StandingCG/" + data.spriteName);
                preLoadedSprites.Add(data.spriteName, sprite);

                data.order = "Change";

                convDatas.Add(data);

                break;
            case "Animation":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }
                val = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                data.name = val;

                data.order = "Animation";

                convDatas.Add(data);

                break;
            case "Delete":
                while (fileData[curIndex] != '<')
                    curIndex += 1;
                while (fileData[curIndex] == '<')
                {
                    curIndex += 1;
                }
                val = ReadUntilTagEnd(fileData, curIndex, out curIndex);

                data.name = val;

                data.order = "Delete";

                convDatas.Add(data);

                break;
        }
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

    private string ReadUntilTagEnd(string data, int startIndex, out int index, bool removeSequence = true)
    {
        string str = "";

        while ((data[startIndex] != '>' && data[startIndex] != '<') ||
            (data[startIndex] == '>' && data[startIndex - 1] == '§') ||
            (data[startIndex] == '<' && data[startIndex - 1] == '§'))
        {
            str += data[startIndex];

            startIndex += 1;
        }

        if (removeSequence)
        {
            str = str.Replace("§", "");
        }

        //Debug.Log(str + " is readed");
        index = startIndex;
        return str;
    }

    private DistracterData ParseDistracters(ref string data, int curIndex, out int index, ref int openedBracket)
    {
        while (data[curIndex] != '<')
            curIndex += 1;
        while (data[curIndex] == '<')
        {
            openedBracket += 1;
            curIndex += 1;
        }

        DistracterData disData = new DistracterData();
        string code = ReadUntilTagEnd(data, curIndex, out curIndex);
        disData.code = int.Parse(code);

        while (data[curIndex] != '>')
            curIndex += 1;
        while (data[curIndex] == '>')
        {
            openedBracket += 1;
            curIndex -= 1;
        }


        while (data[curIndex] != '<')
            curIndex += 1;
        while (data[curIndex] == '<')
        {
            openedBracket += 1;
            curIndex += 1;
        }

        curIndex -= 1;
        ParseNumber(ref data, ref curIndex);
        string numStr = ReadUntilTagEnd(data, curIndex, out curIndex);
        int num = int.Parse(numStr);

        while (data[curIndex] != '>')
            curIndex += 1;
        while (data[curIndex] == '>')
        {
            openedBracket += 1;
            curIndex -= 1;
        }

        int idx = 0;
        disData.distracters = new string[num];
        disData.results = new string[num];
        while (idx < num)
        {
            while (data[curIndex] != '<')
                curIndex += 1;
            while (data[curIndex] == '<')
            {
                openedBracket += 1;
                curIndex += 1;
            }

            int curStsartIndex = curIndex;
            code = ReadUntilTagEnd(data, curIndex, out curIndex);
            int startBracket = 0;
            bool chk;

            switch (code)
            {
                case "If":
                    chk = ParseCondition(ref data, ref curIndex, ref openedBracket, false);

                    code = ReadUntilTagEnd(data, curIndex, out curIndex);
                    if(chk)
                        disData.distracters[idx] = code;

                    startBracket = openedBracket;
                    do
                    {
                        while (data[curIndex] != '<')
                        {
                            curIndex += 1;
                        }

                        while (data[curIndex] == '<')
                        {
                            if(chk)
                                disData.results[idx] += "<";
                            openedBracket += 1;
                            curIndex += 1;
                        }

                        string link = ReadUntilTagEnd(data, curIndex, out curIndex, false);

                        if(chk)
                            disData.results[idx] += link;

                        while (openedBracket > startBracket && data[curIndex] == '>')
                        {
                            if(chk)
                                disData.results[idx] += ">";
                            openedBracket -= 1;
                            curIndex += 1;
                        }
                    }
                    while (openedBracket > startBracket);

                    if (chk)
                        idx += 1;
                    break;
                case "Parameter":
                    ParseParameters(ref data, curStsartIndex, curIndex, out curIndex, ref openedBracket);

                    break;
                default:
                    disData.distracters[idx] = code;

                    startBracket = openedBracket;
                    do
                    {
                        while (data[curIndex] != '<')
                        {
                            curIndex += 1;
                        }

                        while (data[curIndex] == '<')
                        {
                            disData.results[idx] += "<";
                            openedBracket += 1;
                            curIndex += 1;
                        }

                        string link = ReadUntilTagEnd(data, curIndex, out curIndex, false);

                        disData.results[idx] += link;

                        while (openedBracket > startBracket && data[curIndex] == '>')
                        {
                            disData.results[idx] += ">";
                            openedBracket -= 1;
                            curIndex += 1;
                        }
                    }
                    while (openedBracket > startBracket);

                    idx += 1;
                    break;
            }
        }


        index = curIndex;
        return disData;
    }

    private void ParseAlignData()
    {
        ConversationFileDataAlign align =
            (ConversationFileDataAlign)convDatas[curConvIndex];

        switch (align.align)
        {
            case "CenterCenter":
                convText.Text.alignment = TextAnchor.MiddleCenter;

                break;
            case "LeftTop":
                convText.Text.alignment = TextAnchor.UpperLeft;

                break;
        }
    }

    public void StartConversationEvent(string eventName)
    {
        InitConversationDatas();
        InitConversationEvent();
        SetCurEventBasicPath(eventName);
        this.eventName = eventName;
        SceneManager.Instance.ChangeScene("ConversationScene");
    }

    //다음 텍스트를 보이는 것과 관련된 가공을 맡는다.
    public void ShowText()
    {
        if (curConvIndex < convDatas.Count)
        {
            string code = convDatas[curConvIndex].firstCode;

            if (convText.CanBeUsed)
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
                            convText.ClearText();
                            ParseAlignData();

                            break;
                        case "Prefab":
                            ParsePrefab((ConversationFileDataPrefab)convDatas[curConvIndex]);

                            break;
                        case "File":
                            string link = ((ConversationFileDataFile)convDatas[curConvIndex]).fileName;

                            InitConversationDatas();
                            ParseConvFile(link);
                            ShowText();
                            return;
                        case "Standing":
                            ParseStandingCgExecute((ConversationFileDataStanding)convDatas[curConvIndex]);

                            break;
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
                    if (convData.talkerCode == -1)
                    {
                        Color clr = talkerBack.color;
                        clr.a = 0;
                        talkerBack.color = clr;
                    }
                    else
                    {
                        Color clr = talkerBack.color;
                        clr.a = 1;
                        talkerBack.color = clr;
                    }
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
        int height = (460 - (int)group.cellSize.y * disData.distracters.Length) / (disData.distracters.Length + 1);

        group.padding.top = height;
        group.spacing = new Vector2(0, height);
    }

    public void DistracterSelected(int distracterNum)
    {
        Debug.Log("Distracter " + distracterNum + " selected");
        UIManager.Instance.SetEnableTouchLayer("Main", true);
        //selectedDistracter.Add(curDistracterCode, distracterNum);
        Destroy(distracterPopup);
        distracterPopup = null;

        ParseDistracterResult(curDistracterResults[distracterNum]);
    }

    private void ParseDistracterResult(string result)
    {
        int curIndex = 0;
        int openedBracket = 0;

        do
        {
            while (result[curIndex] != '<' && result[curIndex] != '>')
            {
                curIndex += 1;
            }
            if (result[curIndex] == '<')
            {
                while (result[curIndex] == '<')
                {
                    openedBracket += 1;
                    curIndex += 1;
                }

                int startIndex = curIndex;
                string code = ReadUntilTagEnd(result, curIndex, out curIndex);

                ParseCodes(code, ref result, startIndex, ref curIndex, ref openedBracket);
            }
            else
            {
                while (curIndex < result.Length && result[curIndex] == '>')
                {
                    openedBracket -= 1;
                    curIndex += 1;
                }
            }
        } while (openedBracket > 0 && curIndex < result.Length);

        ShowText();
    }

    private void ParsePrefab(ConversationFileDataPrefab data)
    {
        GameObject obj;
        if (preLoadedPrefabs.TryGetValue(data.prefabName, out obj))
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

    private void ParseStandingCgExecute(ConversationFileDataStanding data)
    {
        SpriteRenderer renderer;
        Sprite sprite;
        switch (data.order)
        {
            case "Set":
                GameObject obj = new GameObject("CG", new System.Type[] { typeof(SpriteRenderer), typeof(Animator) });
                obj.transform.SetParent(GameManager.Instance.World.transform);
                obj.transform.localPosition = new Vector2(data.xPos, -360);
                obj.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
                
                RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>("Animations/StandingCG/" + data.spriteName);
                if (controller == null)
                {
                    renderer = obj.GetComponent<SpriteRenderer>();
                    sprite = Resources.Load<Sprite>("Sprites/Conversations/StandingCG/" + data.spriteName);
                    renderer.sprite = sprite;
                }
                else
                {
                    obj.GetComponent<Animator>().runtimeAnimatorController = controller;
                }

                renderer = obj.GetComponent<SpriteRenderer>();
                renderer.sortingLayerName = "Room";

                standingCGs.Add(data.name, renderer);
                break;
            case "Change":
                if (standingCGs.TryGetValue(data.name, out renderer))
                {
                    Animator animator = renderer.GetComponent<Animator>();
                    if (animator != null)
                        animator.enabled = false;
                    
                    preLoadedSprites.TryGetValue(data.spriteName, out sprite);
                    renderer.sprite = sprite;
                }
                else
                {
                    Debug.LogError("Could NOT FIND CG Code " + data.name);
                }
                break;
            case "Animation":
                if (standingCGs.TryGetValue(data.name, out renderer))
                {
                    renderer.GetComponent<Animator>().enabled = true;
                }
                else
                {
                    Debug.LogError("Could NOT FIND CG Code " + data.name);
                }
                break;
            case "Delete":
                if (standingCGs.TryGetValue(data.name, out renderer))
                {
                    standingCGs.Remove(data.name);
                    Destroy(renderer.gameObject);
                }
                else
                {
                    Debug.LogError("Could NOT FIND CG Code " + data.name);
                }

                break;
        }
    }

    private void SetCurEventBasicPath(string eventName)
    {
        curEventBasicPath = "Datas/Conversations/" + eventName + "/";
    }

    public void InitConversationDatas()
    {
        convDatas.Clear();
        preLoadedPrefabs = new Dictionary<string, GameObject>();
        preLoadedSprites = new Dictionary<string, Sprite>();
        curConvIndex = 0;
    }

    public int GetParameter(string parameterName)
    {
        int val = -1;
        if (parameters.TryGetValue(parameterName, out val))
        {
            return val;
        }
        else
        {
            return val;
        }
    }

    public void InitConversationEvent()
    {
        parameters.Clear();
        standingCGs.Clear();
    }

    private void AddConvData(ConversationFileDataBase data)
    {
        if (convDataAddIndex == -1)
        {
            convDatas.Add(data);
        }
        else
        {
            convDatas.Insert(convDataAddIndex, data);
        }
    }
}