using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{
    private bool opened = false;
    private Image image = null;
    private Button button = null;
    private Sprite statNormal = null;
    private Sprite statPushed = null;
    private Sprite closeNormal = null;
    private Sprite closePushed = null;
    private SpriteState state;

	// Use this for initialization
	void Start ()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        state = button.spriteState;
        statNormal = image.sprite;
        statPushed = state.pressedSprite;
        closeNormal = Resources.Load<Sprite>("Sprites/UI/UI_Button_Close_Normal");
        closePushed = Resources.Load<Sprite>("Sprites/UI/UI_Button_Close_Pushed");
	}
	
    public void OnClick()
    {
        if(!opened)
        {
            GameManager.Instance.OpenStatPopup();
            image.sprite = closeNormal;
            state.pressedSprite = closePushed;
            button.spriteState = state;
        }
        else
        {
            GameManager.Instance.CloseStatPopup();
            image.sprite = statNormal;
            state.pressedSprite = statPushed;
            button.spriteState = state;
        }
        opened = !opened;
    }
}
