using UnityEngine;
using System.Collections;

public class CheatDateYear : MonoBehaviour
{
    public void OnClick()
    {
        Date date = GameManager.Instance.GameDate;

        date.Year += 1;

        GameManager.Instance.GameDate = date;
    }
}
