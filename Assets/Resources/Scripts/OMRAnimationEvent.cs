using UnityEngine;
using System.Collections;

public class OMRAnimationEvent : MonoBehaviour
{
    public void AnimationEnd()
    {
        LoadExam.Instance.AnimationEnded();
    }
}
