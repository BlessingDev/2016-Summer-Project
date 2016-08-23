using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundOnOffButton : MonoBehaviour {

    private Sprite offNormal;
    private Sprite offPushed;
    private Sprite onNormal;
    private Sprite onPushed;

    private Button button;

	// Use this for initialization
	void Start ()
    {
        button = GetComponent<Button>();

        onNormal = GetComponent<Image>().sprite;
        onPushed = button.spriteState.pressedSprite;
        offNormal = Resources.Load<Sprite>("Sprites/PausePopup/UI_Button_Sound_Off_Normal");
        offPushed = Resources.Load<Sprite>("Sprites/PausePopup/UI_Button_Sound_Off_Pushed");


        if (SoundManager.Instance.IsSoundOff)
        {
            button.image.sprite = offNormal;
            SpriteState state = button.spriteState;
            state.pressedSprite = offPushed;
            button.spriteState = state;
        }
    }
	
    public void OnClick()
    {
        if(SoundManager.Instance.IsSoundOff)
        {
            button.image.sprite = onNormal;
            SpriteState state = button.spriteState;
            state.pressedSprite = onPushed;
            button.spriteState = state;
        }
        else
        {
            button.image.sprite = offNormal;
            SpriteState state = button.spriteState;
            state.pressedSprite = offPushed;
            button.spriteState = state;
        }

        SoundManager.Instance.IsSoundOff = !SoundManager.Instance.IsSoundOff;
    }
}
