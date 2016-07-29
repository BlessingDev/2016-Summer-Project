using UnityEngine;
using System.Collections;

public class PauseExitButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.ResumeGame();
    }
}
