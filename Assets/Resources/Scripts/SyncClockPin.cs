using UnityEngine;
using System.Collections;

public class SyncClockPin : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        int time = (int)SchedulingManager.Instance.GameTime;

        int degree = 30 * (time % 12);
        Vector3 angles = transform.localEulerAngles;
        angles.z = degree;
        transform.localEulerAngles = angles;
	}
}
