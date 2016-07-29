using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.PauseGame();
    }
}
