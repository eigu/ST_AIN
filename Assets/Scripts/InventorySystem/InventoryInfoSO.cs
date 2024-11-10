using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventoryInfo", menuName = "InventoryInfoSO", order = 1)]
public class InventoryInfoSO : ScriptableObject
{
    public string inventoryName;
    public int maxInventoryCapacity = 10;
    public List<WasteInfoSO> predefinedContents = new List<WasteInfoSO>();
    public Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

    private void Awake()
    {
        if (predefinedContents.Count <= 0) return;
        foreach (var wasteInfo in predefinedContents)
        {
            Add(wasteInfo, 1);
        }
    }

    public void Add(WasteInfoSO wasteInfoSo, int quantity)
    {
        if (maxInventoryCapacity < inventory.Count)
        {
            Debug.LogError("Maximum capacity reached, can't add: " + wasteInfoSo.ID);
            return;
        }
        
        if (inventory.ContainsKey(wasteInfoSo.ID))
        {
            inventory[wasteInfoSo.ID].quantity += quantity;
        }
        else
        {
            inventory[wasteInfoSo.ID] = new InventoryItem(wasteInfoSo, quantity);
        }
    }

    public void Remove(string wasteID, int quantity)
    {
        if (inventory.ContainsKey(wasteID))
        {
            var item = inventory[wasteID];
            item.quantity -= quantity;
            if (item.quantity <= 0)
            {
                inventory.Remove(wasteID);
            }
        }
    }

    
}
