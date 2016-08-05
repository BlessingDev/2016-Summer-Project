using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomNumberText : MonoBehaviour {

    [SerializeField]
    private string spritePath = "";
    private Dictionary<char, Sprite> spritesDic;
    private List<Image> showImages;
    private Dictionary<char, int> widthDic;
    private string text = "0";
    public string Text
    {
        get
        {
            return text;
        }
        set
        {
            text = value;
            UpdateText();
        }
    }
    public float alpha
    {
        get
        {
            return showImages[0].color.a;
        }
        set
        {
            foreach(var iter in showImages)
            {
                Color color = iter.color;
                color.a = value;
                iter.color = color;
            }
        }
    }
    private Rect entireRect;
    public Rect rect
    {
        get
        {
            return entireRect;
        }
    }
    [SerializeField]
    private TextAlignment alignment = TextAlignment.Left;
    public TextAlignment Alignment
    {
        get
        {
            return alignment;
        }
        set
        {
            alignment = value;
            SetAlignment();
        }
    }
    private bool inited = false;

	// Use this for initialization
	void Start ()
    {
        if(!inited)
        {
            inited = true;
            spritesDic = new Dictionary<char, Sprite>();
            widthDic = new Dictionary<char, int>();
            showImages = new List<Image>();

            var sprites = Resources.LoadAll<Sprite>(spritePath);
            for (int i = 0; i < sprites.Length; i += 1)
            {
                spritesDic.Add(sprites[i].name[0], sprites[i]);
                widthDic.Add(sprites[i].name[0], (int)sprites[i].rect.width);
            }
            UpdateText();

            if (sprites == null)
            {
                enabled = false;
            }
        }
	}

    void AddCharactor()
    {
        GameObject obj = new GameObject("Num" + showImages.Count, new System.Type[] { typeof(Image) });
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        showImages.Add(obj.GetComponent<Image>());
    }

    void RemoveCharactor()
    {
        if(showImages.Count > 0)
        {
            Destroy(showImages[showImages.Count - 1].gameObject);
            showImages.RemoveAt(showImages.Count - 1);
        }
        else
        {
            Debug.LogError("You tried to Remove object from Empty List");
        }
    }

    void UpdateText()
    {
        if (!inited)
            Start();

        while(text.Length != showImages.Count)
        {
            if(text.Length > showImages.Count)
            {
                AddCharactor();
            }
            else
            {
                RemoveCharactor();
            }
        }

        text = text.Replace('.', '⊙');
        entireRect.width = 0;
        entireRect.height = 0;
        for(int i = 0; i < text.Length; i += 1)
        {
            Sprite sprite;
            if(spritesDic.TryGetValue(text[i], out sprite))
            {
                showImages[i].transform.localPosition = Vector3.zero;
                showImages[i].sprite = sprite;
                showImages[i].SetNativeSize();
                int curWidth;
                widthDic.TryGetValue(text[i], out curWidth);
                entireRect.width += curWidth;

                if (sprite.rect.height >= entireRect.height)
                    entireRect.height = sprite.rect.height;

                if(i >= 1)
                {
                    int befWidth;
                    widthDic.TryGetValue(text[i - 1], out befWidth);

                    showImages[i].transform.localPosition = 
                        new Vector3(showImages[i - 1].transform.localPosition.x + befWidth / 2 + curWidth / 2, 0);
                }
            }
            else
            {
                Debug.LogError("Couldn't Find " + text[i] + " sprite");
            }
        }

        SetAlignment();
    }

    void SetAlignment()
    {
        //alignment;
        float xOffset = 0;
        switch (alignment)
        {
            case TextAlignment.Right:
                xOffset = rect.width - ((rect.width / showImages.Count) / 2);
                break;
            case TextAlignment.Center:
                xOffset = (rect.width / 2) - ((rect.width / showImages.Count) / 2);
                break;
        }

        foreach (var iter in showImages)
        {
            Vector3 pos = iter.transform.localPosition;
            pos.x -= xOffset;
            iter.transform.localPosition = pos;
        }
    }
}
