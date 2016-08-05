using UnityEngine;
using System.Collections;

public class NoteLeftButton : MonoBehaviour {

    public void OnClick()
    {
        NoteManager.Instance.TurnOverLeft();
    }
}
