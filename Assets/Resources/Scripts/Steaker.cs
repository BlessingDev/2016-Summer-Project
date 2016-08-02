using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Steaker : MonoBehaviour
{
    [SerializeField]
    ScheduleType type;   

    private int num = 0;
    public int Num
    {
        get
        {
            return num;
        }
        set
        {
            if(num != -1)
            {
                num = value;
            }
        }
    }
}
