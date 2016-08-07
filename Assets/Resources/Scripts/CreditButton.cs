using UnityEngine;
using System.Collections;

public class CreditButton : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.OpenCredit();
    }
}
