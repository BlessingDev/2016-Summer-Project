using UnityEngine;
using System.Collections;

public class SteakerPlateBackwardButton : MonoBehaviour
{
    public void OnClick()
    {
        SchedulingManager.Instance.SteakerPlateBackward();
    }
}
