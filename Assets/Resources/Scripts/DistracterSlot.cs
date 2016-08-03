using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistracterSlot : MonoBehaviour
{
    [SerializeField]
    private Text text = null;
    public Text Text
    {
        get
        {
            return text;
        }
    }
    public int distracterNum = 0;


    public void OnClick()
    {
    }
}
