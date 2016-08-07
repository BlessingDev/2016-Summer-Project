using UnityEngine;
using System.Collections;

public class PauseExitButton : MonoBehaviour
{
    public void OnClick()
    {
        if (GameManager.Instance.Credit != null)
            GameManager.Instance.CloseCredit();
        else
            GameManager.Instance.ResumeGame();
    }
}
