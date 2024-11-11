using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    public void EmptyInventory()
    {
        foreach (var item in Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items)
        {
            item.Subtract(item.Count);
            Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(item, 0);
        }
        //Bootstrap.Instance.GameData.Inventory.InventoryScreen.SetCount(Bootstrap.Instance.GameData.Inventory.ItemsInventory.Count());
    }

    public void AddAllResources()
    {
        foreach (var item in Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items)
        {
            if (item.Id == "Helper") continue;
            item.Add(100);
            Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(item, item.Count);
        }
    }
}
