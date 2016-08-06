using UnityEngine;
using System.Collections;

public class MapChecker : MonoBehaviour
{
    public void OnClick()
    {
        MovementManager.Instance.FindPath();
    }
}
