using UnityEngine;
using System.Collections.Generic;

public class ShopManager : Manager<ShopManager>
{
    [SerializeField]
    private GameObject preShopPopup = null;
    private GameObject shopPopup = null;
    public GameObject ShopPopup
    {
        get
        {
            return shopPopup;
        }
    }

    private Dictionary<int, GameObject[]> shoppingList;
    private ScaleEffecter[] effecters = null;

    private int shopType = 0;       // 어떤 종류의 제품을 구입할건지
    public int ShopType
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

    private int shopNum = 0;        // 어떤 등급의 제품을 선택중인지
    public int ShopNum
    {
        get
        {
            return shopNum;
        }
        set
        {
            shopNum = value;
        }
    }

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
            shoppingList = new Dictionary<int, GameObject[]>();

            for (int i = 1; i <= 5; i += 1)
            {
                var objs = Resources.LoadAll<GameObject>("Prefabs/Shop/" + i.ToString() + "/");
                shoppingList.Add(i, objs);
            }

            effecters = FindObjectsOfType<ScaleEffecter>();

            if (preShopPopup == null)
            {
                Debug.LogWarning("The Prefab NOT PREPARED");
            }
        }
    }

    public void OpenShopPopup(int shopType)
    {
        this.shopType = shopType;

        UIManager.Instance.SetEnableTouchLayer("Main", true);
        for(int i = 0; i < effecters.Length; i += 1)
        {
            effecters[i].enabled = false;
        }

        shopPopup = Instantiate(preShopPopup);
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

        Destroy(shopPopup);
        shopPopup = null;
    }
}
