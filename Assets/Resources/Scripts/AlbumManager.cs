using UnityEngine;
using System.Collections.Generic;

public class AlbumManager : Manager<AlbumManager>
{
    private Dictionary<string, bool> endingList;
    [SerializeField]
    private GameObject preAlbumPopup;
    private List<GameObject> preSlots;
    [SerializeField]
    private GameObject preReturnButton;

    void Start()
    {
        Instance.Init();
        if (preAlbumPopup == null || preReturnButton == null)
        {
            Debug.LogError("Prefabs Are NOT READY");
            return;
        }

        endingList = new Dictionary<string, bool>();

        endingList.Add("Military Ending", false);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            == SceneManager.Instance.GetLevel("AlbumScene"))
        {
            InitAlbumScene();
        }
    }

    public override void OnLevelWasLoaded(int level)
    {
        base.OnLevelWasLoaded(level);

        if (level == SceneManager.Instance.GetLevel("AlbumScene"))
        {
            InitAlbumScene();
        }
    }

    public void InitAlbumScene()
    {
        GameObject obj = Instantiate(preAlbumPopup);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = Vector2.zero;
        obj.transform.localScale = Vector3.one;

        obj = Instantiate(preReturnButton);
        obj.transform.SetParent(UIManager.Instance.Canvas.transform);
        obj.transform.localPosition = preReturnButton.transform.localPosition;
        obj.transform.localScale = preReturnButton.transform.localScale;
    }

    public bool IsEndingOpened(string endingName)
    {
        bool chk;
        if (endingList.TryGetValue(endingName, out chk))
        {
            return chk;
        }
        else
        {
            Debug.LogWarning("Ending " + endingName + "DOESN'T EXIST");
            return false;
        }
    }

    public void SetEnding(string endingName, bool chk)
    {
        if (endingList.ContainsKey(endingName))
        {
            endingList.Remove(endingName);
            endingList.Add(endingName, chk);
        }
        else
        {
            Debug.LogError("There is NO Ending " + endingName);
        }
    }
}
