using UnityEngine;
using System.Collections;

public class SceneChangeButton : MonoBehaviour
{
    public string sceneName = "";

    public void OnClick()
    {
        SceneManager.Instance.ChangeScene(sceneName);
    }
}
