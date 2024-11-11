using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryScreen : UIScreen
{
    [SerializeField] Transform resourceUIContainer;
    [SerializeField] Transform resourceUIPrefab;
    [SerializeField] TextMeshProUGUI resourcesCount;
    [SerializeField] List<ResourceUIComponent> resources;

    int maxItems;
    public void createItemUI(Item item)
    {
        var res=Instantiate(resourceUIPrefab,resourceUIContainer);
        res.GetComponent<ResourceUIComponent>().SetItem(item);
        resources.Add(res.GetComponent<ResourceUIComponent>());
    }

    public void UpdateUI(Item item,float count)
    {
        foreach (var res in resources)
        {
            if (item.Id==res.ItemId)
            {
                res.UpdateResCount(count);
            }
        }
    }
}
