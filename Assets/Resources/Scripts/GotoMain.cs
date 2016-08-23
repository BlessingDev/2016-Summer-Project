using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GotoMain : MonoBehaviour
{
    private bool reserved = false;
    private Image image = null;
    private Button button = null;
    private Sprite normalNormal = null;
    private Sprite normalPushed = null;
    private Sprite reservedNormal = null;
    private Sprite reservedPushed = null;
    private bool lockButton = false;
    private bool inited = false;

    void Start()
    {
        if(!inited)
        {
            inited = true;
            image = GetComponent<Image>();
            button = GetComponent<Button>();

            normalNormal = image.sprite;
            var state = button.spriteState;
            normalPushed = state.pressedSprite;
            reservedNormal = Resources.Load<Sprite>("Sprites/ScheduleUI/Schedule_Cancel_Reserve_Normal");
            reservedPushed = Resources.Load<Sprite>("Sprites/ScheduleUI/Schedule_Cancel_Reserve_Pushed");
        }
    }

    public void OnClick()
    {
        if(!lockButton)
        {
            if (!reserved)
            {
                if (!inited)
                    Start();

                image.sprite = reservedNormal;
                var state = button.spriteState;
                state.pressedSprite = reservedPushed;
                button.spriteState = state;
            }
            else
            {
                image.sprite = normalNormal;
                var state = button.spriteState;
                state.pressedSprite = normalPushed;
                button.spriteState = state;
            }

            SchedulingManager.Instance.GotoMainReserved = !reserved;
            reserved = !reserved;
        }
    }

    public void LockButton()
    {
        lockButton = true;
    }
}
