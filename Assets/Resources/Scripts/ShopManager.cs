using UnityEngine;
using System.Collections.Generic;

public class ShopManager : Manager<ShopManager>
{
    [SerializeField]
    private GameObject preShopPopup = null;
    private ShopPopup shopPopup = null;
    public GameObject ShopPopup
    {
        get
        {
            return shopPopup.gameObject;
        }
    }

    private Dictionary<SkinType, GameObject[]> shoppingList;
    private ScaleEffecter[] effecters = null;

    private SkinType shopType = 0;       // 어떤 종류의 제품을 구입할건지
    public SkinType ShopType
    {
        get
        {
            return shopType;
        }
        set
        {
            shopType = value;
        }
    }

    private string selectedSkinName;
    public string SelectedSkinName
    {
        get
        {
            return selectedSkinName;
        }
        set
        {
            selectedSkinName = value;
            RefreshExplanation();
            RefreshIcon();
            CheckButton();
        }
    }

    private Dictionary<SkinType, Dictionary<string, int>> priceDic;
    private Dictionary<SkinType, Dictionary<string, bool>> buyCheck;

    public override void Init()
    {
        Start();
        base.Init();
    }

    // Use this for initialization
    void Start ()
    {
        if(!IsInited)
        {
            priceDic = new Dictionary<SkinType, Dictionary<string, int>>();
            buyCheck = new Dictionary<SkinType, Dictionary<string, bool>>();
            shoppingList = new Dictionary<SkinType, GameObject[]>();

            for (int i = 1; i <= 5; i += 1)
            {
                var objs = Resources.LoadAll<GameObject>("Prefabs/Shop/" + ((SkinType)i).ToString() + "/");
                shoppingList.Add((SkinType)i, objs);

                if((SkinType)i != SkinType.Costume)
                {
                    Dictionary<string, bool> checkDic = new Dictionary<string, bool>();
                    Dictionary<string, int> priceNameDic = new Dictionary<string, int>();
                    for (int j = 0; j < objs.Length; j += 1)
                    {
                        ShopSlot slot = objs[j].GetComponent<ShopSlot>();
                        slot.GetSpriteName();
                        // 세이브 기능 구현시 고쳐야 할 부분
                        checkDic.Add(slot.SpriteName, false);

                        priceNameDic.Add(slot.SpriteName, slot.Price);
                    }

                    buyCheck.Add((SkinType)i, checkDic);
                    priceDic.Add((SkinType)i, priceNameDic);
                }
                else
                {
                    Dictionary<string, bool> checkDic = new Dictionary<string, bool>();
                    Dictionary<string, int> priceNameDic = new Dictionary<string, int>();
                    for (int j = 0; j < objs.Length; j += 1)
                    {
                        CostumeSlot slot = objs[j].GetComponent<CostumeSlot>();
                        // 세이브 기능 구현시 고쳐야 할 부분
                        checkDic.Add(slot.CostumeCode.ToString(), false);

                        priceNameDic.Add(slot.CostumeCode.ToString(), slot.Price);
                    }

                    buyCheck.Add((SkinType)i, checkDic);
                    priceDic.Add((SkinType)i, priceNameDic);
                }
            }

            effecters = FindObjectsOfType<ScaleEffecter>();

            if (preShopPopup == null)
            {
                Debug.LogWarning("The Prefab NOT PREPARED");
            }
        }
    }

    public void OpenShopPopup(SkinType shopType)
    {
        this.shopType = shopType;

        UIManager.Instance.SetEnableTouchLayer("Main", true);
        for(int i = 0; i < effecters.Length; i += 1)
        {
            effecters[i].enabled = false;
        }

        shopPopup = Instantiate(preShopPopup).GetComponent<ShopPopup>();
        shopPopup.transform.parent = UIManager.Instance.Canvas.transform;
        shopPopup.transform.localPosition = Vector3.zero;
        shopPopup.transform.localScale = Vector3.one;

        GameObject[] objs;
        if(shoppingList.TryGetValue(shopType, out objs))
        {
            RectTransform trans = shopPopup.GetComponent<ShopPopup>().ScrolledRect.GetComponent<RectTransform>();
            int width = objs.Length * 150 + (objs.Length - 1) * 50;
            Vector2 offset = trans.offsetMax;
            offset.x = trans.offsetMin.x + width + 20;
            trans.offsetMax = offset;

            for (int i = 0; i < objs.Length; i += 1)
            {
                GameObject obj = Instantiate(objs[i]);
                obj.transform.SetParent(trans.transform);
                obj.transform.localScale = Vector3.one;
            }
        }
    }

    public void ClosePopup()
    {
        UIManager.Instance.SetEnableTouchLayer("Main", true);
        for (int i = 0; i < effecters.Length; i += 1)
        {
            effecters[i].enabled = true;
        }

        Destroy(shopPopup.gameObject);
        shopPopup = null;
    }

    public void RefreshExplanation()
    {

    }

    public void RefreshIcon()
    {

    }

    public void CheckButton()
    {
        Dictionary<string, bool> checkDic;
        buyCheck.TryGetValue(shopType, out checkDic);
        bool check;
        if(checkDic.TryGetValue(selectedSkinName, out check))
        {
            if(check)
            {
                shopPopup.Bought();
            }
            else
            {
                shopPopup.NotBought();
            }
        }
        else
        {
            Debug.LogError("Could not FIND name " + selectedSkinName);
        }
    }

    public void Purchase()
    {
        Dictionary<string, int> priceNameDic;
        priceDic.TryGetValue(shopType, out priceNameDic);
        int price;
        if (priceNameDic.TryGetValue(selectedSkinName, out price))
        {
            if(price <= GameManager.Instance.Money)
            {
                GameManager.Instance.Money -= price;

                Dictionary<string, bool> checkDic;
                buyCheck.TryGetValue(shopType, out checkDic);
                if (checkDic.ContainsKey(selectedSkinName))
                {
                    checkDic.Remove(selectedSkinName);
                    checkDic.Add(selectedSkinName, true);
                }
                else
                {
                    Debug.LogError("Could not FIND name " + selectedSkinName);
                }

                shopPopup.Bought();
            }
        }
        else
        {
            Debug.LogError("Could not FIND name " + selectedSkinName);
        }
    }

    public void Use()
    {
        if(shopType < SkinType.Costume)
        {
            GameManager.Instance.SetSkinName(shopType, selectedSkinName);
        }
        else
        {
            GameManager.Instance.CostumeCode = int.Parse(selectedSkinName);
        }
    }
}
