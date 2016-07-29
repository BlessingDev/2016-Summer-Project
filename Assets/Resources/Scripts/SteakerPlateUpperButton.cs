using UnityEngine;
using System.Collections;

public class SteakerPlateUpperButton : MonoBehaviour
{
    public void OnClick()
    {
        SchedulingManager.Instance.SteakerPlateUpper();
    }
}
