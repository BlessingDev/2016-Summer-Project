using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TutorialManager : Manager<TutorialManager>
{
    [SerializeField]
    private GameObject preTutorial;

    private Dictionary<string, Sprite> preloadedImages;
    private Dictionary<string, bool> imageCheck;

    // Use this for initialization
    void Start ()
    {
	    if(!inited)
        {
            preloadedImages = new Dictionary<string, Sprite>();
            imageCheck = new Dictionary<string, bool>();

            var sprites = Resources.LoadAll<Sprite>("Sprites/Tutorials/");
            for(int i = 0; i < sprites.Length; i += 1)
            {
                preloadedImages.Add(sprites[i].name, sprites[i]);
                imageCheck.Add(sprites[i].name, false);
            }

            var check = Instance;
        }
	}

    public void TryTutorial(string tutorialName)
    {
        if(!imageCheck[tutorialName])
        {
            imageCheck[tutorialName] = true;

            Sprite sprite = preloadedImages[tutorialName];

            GameObject obj = Instantiate(preTutorial);
            obj.transform.SetParent(UIManager.Instance.Canvas.transform);
            obj.transform.localPosition = Vector2.zero;
            obj.transform.localScale = Vector3.one;

            Image image = obj.GetComponent<Image>();
            image.sprite = sprite;
            image.SetNativeSize();
        }
    }
}
