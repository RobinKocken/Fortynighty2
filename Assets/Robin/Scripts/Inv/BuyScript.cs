using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyScript : MonoBehaviour
{
    public InventoryManager inventory;

    [Header("Options")]
    public Item item;
    public int amount;
    public Item currency;
    public int price;

    public Image icon;
    public TMP_Text itemName;
    public TMP_Text priceText;

    public int slotNumber;
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<InventoryManager>();

        icon.sprite = item.icon;
        itemName.text = amount + " "+ item.name;
        priceText.text = price.ToString();

    }

    public void Buy()
    {
        slotNumber = inventory.CheckForItem(currency, slotNumber);

        if(slotNumber != -1)
        {
            if(inventory.slots[slotNumber].GetComponent<Slot>().itemData != null)
            {
                if(0 < inventory.slots[slotNumber].GetComponent<Slot>().amount)
                {
                    if(price >= price - inventory.slots[slotNumber].GetComponent<Slot>().amount)
                    {
                        GameStats.scrapUsed += price;

                        inventory.RemoveItem(currency, price);
                        inventory.AddItem(item, amount);
                    }
                }
            }
        }

    }
}
