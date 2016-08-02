using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SchedulePopup : MonoBehaviour
{
    [SerializeField]
    GameObject parameters;
    [SerializeField]
    Animator animator;
    [SerializeField]
    private Image failOrSuccess;
    private Sprite fail;
    private Sprite success;
    private bool reserved = false;


    // Use this for initialization
    void Start()
    {
        if (parameters == null)
        {
            Debug.LogError("parameters not ready");
            enabled = false;
        }
        if (animator == null)
        {
            Debug.LogError("animator not ready");
            enabled = false;
        }
        if (failOrSuccess == null)
        {
            Debug.LogError("fail or success not ready");
            enabled = false;
        }

        failOrSuccess.enabled = false;
        fail = Resources.Load<Sprite>("Sprites/ScheduleUI/fail");
        success = Resources.Load<Sprite>("Sprites/ScheduleUI/Success");
    }

    public void InitParameters()
    {
        for(int i = 0; i < parameters.transform.childCount; i += 1)
        {
            Transform obj = parameters.transform.GetChild(i);
            Destroy(obj.gameObject);
        }
    }

    public void AddParameter(GameObject obj)
    {
        obj.transform.SetParent(parameters.transform);
        obj.transform.localScale = new Vector3(0.7f, 0.7f, 1);
    }

    public void SetAnimationType(int type)
    {
        animator.SetInteger("AnimationType", type);
    }

    public void ShowFailOrSuccess(bool success)
    {
        failOrSuccess.enabled = true;
        if(success)
        {
            failOrSuccess.sprite = this.success;
        }
        else
        {
            failOrSuccess.sprite = fail;
        }

    }

    public void DisappearFailOrSuccess()
    {
        if(!reserved)
            StartCoroutine(CorDisappearFailOrSuccess());
    }

    IEnumerator CorDisappearFailOrSuccess()
    {
        reserved = true;
        yield return new WaitForSeconds(0.15f);

        failOrSuccess.enabled = false;
        reserved = false;
    }
}
