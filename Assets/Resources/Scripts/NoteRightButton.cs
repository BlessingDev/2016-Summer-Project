using UnityEngine;
using System.Collections;

public class NoteRightButton : MonoBehaviour {

    public void OnClick()
    {
        NoteManager.Instance.TurnOverRight();
    }
}
