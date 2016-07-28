using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteakerPlate : MonoBehaviour
{
    public void MovePlate(int direction)
    {
        StartCoroutine(CorMovePlate(direction));
    }

    IEnumerator CorMovePlate(int direction)
    {
        float speed = 300;
        while(true)
        {
            if(!GameManager.Instance.IsPause)
            {
                Vector2 pos = transform.localPosition;

                if(direction == 1)
                {
                    pos.x -= speed * Time.smoothDeltaTime;

                    if(pos.x <= -320)
                    {
                        pos.x = -320;

                        transform.localPosition = pos;
                        SchedulingManager.Instance.MoveScheduleListEnded();
                        StopAllCoroutines();
                        break;
                    }
                    else
                    {
                        transform.localPosition = pos;
                    }
                }
                else if (direction == 2)
                {
                    pos.x += speed * Time.smoothDeltaTime;

                    if (pos.x >= 320)
                    {
                        pos.x = 320;

                        transform.localPosition = pos;
                        SchedulingManager.Instance.MoveScheduleListEnded();
                        StopAllCoroutines();
                        break;
                    }
                    else
                    {
                        transform.localPosition = pos;
                    }
                }
            }

            yield return null;
        }
    }
}
