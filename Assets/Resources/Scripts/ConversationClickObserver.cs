using UnityEngine;
using System.Collections;

public class ConversationClickObserver : MonoBehaviour {

    public void OnClick()
    {
        ConversationManager.Instance.ShowText();
    }
}
