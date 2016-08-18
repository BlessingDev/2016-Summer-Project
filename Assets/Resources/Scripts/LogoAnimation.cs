using UnityEngine;
using System.Collections;

public class LogoAnimation : MonoBehaviour
{
    Animator animator = null;

	// Use this for initialization
	void Start ()
    {
        animator = GetComponent<Animator>();

        if(animator == null)
        {
            Debug.LogError("Animator is NULL");
            return;
        }

        StartCoroutine(LogoFade());
	}
	
    private IEnumerator LogoFade()
    {
        yield return new WaitForSeconds(2f);

        animator.SetBool("Fade", true);
    }

    public void AnimationEnded()
    {
        SceneManager.Instance.ChangeScene("MainScene");
    }
}
