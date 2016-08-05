using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ButtonClick : MonoBehaviour {

    public void ClickTrue()
    {
        LoadExam.Instance.AnswerMarked(true);
    }

    public void ClickFalse()
    {
        LoadExam.Instance.AnswerMarked(false);
    }
}
