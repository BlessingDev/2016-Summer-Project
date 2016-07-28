using UnityEngine;
using System.Collections;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "";

    public void OnClick()
    {
        SceneManager.Instance.ChangeScene(sceneName);
    }
}
