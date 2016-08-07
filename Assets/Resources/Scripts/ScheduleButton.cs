using UnityEngine;
using System.Collections;

public class ScheduleButton : MonoBehaviour {
    
    public string sceneName = "ScheduleScene";
    public bool isConvEvent = false;

    public void OnClick()
    {
        if(!isConvEvent)
        {
            SceneManager.Instance.ChangeScene(sceneName);
        }
        else
        {
            EventManager.Instance.SetCurEvent(sceneName);
            ConversationManager.Instance.StartConversationEvent(sceneName);
        }
    }
}
