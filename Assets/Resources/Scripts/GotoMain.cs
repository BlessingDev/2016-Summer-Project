using UnityEngine;
using System.Collections;

public class GotoMain : MonoBehaviour
{
    public void OnClick()
    {
        SchedulingManager.Instance.GotoMainReserved = true;
    }
}
