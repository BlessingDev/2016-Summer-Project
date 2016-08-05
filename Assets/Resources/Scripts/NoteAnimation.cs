using UnityEngine;
using System.Collections;

public class NoteAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if(animator == null)
        {
            Debug.LogError("This Object DOESN'T HAVE Animator Component");
            enabled = false;
            return;
        }
    }

    public void OnAnimationEnded()
    {
        animator.SetBool("Left", false);
        animator.SetBool("Right", false);
        NoteManager.Instance.AnimationEnded();
    }
}
