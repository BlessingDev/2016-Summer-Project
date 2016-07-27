using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour , IHasChanged{
    [SerializeField] Transform slots;
    [SerializeField] Text inventoryText;


	// Use this for initialization
	void Start () {
        HasChangde();
	}


    public void HasChangde()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(" - ");
        foreach(Transform slotTransfrom in slots)
        {
            GameObject item = slotTransfrom.GetComponent<Slot>().item;
            if(item)
            {
                builder.Append(item.name);
                builder.Append(" - ");
            }
        }
        inventoryText.text = builder.ToString();
    }
}

namespace UnityEngine.EventSystems
{
    public interface IHasChanged : IEventSystemHandler
    {
        void HasChangde();
    }
}