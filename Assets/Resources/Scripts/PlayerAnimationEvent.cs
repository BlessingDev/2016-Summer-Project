using UnityEngine;
using System.Collections;

public class PlayerAnimationEvent : MonoBehaviour {

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void EmbrassingEnded()
    {
        animator.SetBool("Embrassing", false);
        MovementManager.Instance.enabled = true;
    }
}
