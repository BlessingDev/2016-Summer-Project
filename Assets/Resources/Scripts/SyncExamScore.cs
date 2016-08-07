using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SyncExamScore : MonoBehaviour {

    private Text text = null;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();

        if (text == null)
        {
            Debug.LogError("This Object Doesn't HAVE Text Component");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = GameManager.Instance.LatestScore.ToString();
    }
}
