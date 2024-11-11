using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : GameSystem
{
    [Header("Look")]
    [SerializeField] ItemsInventory inventory;

    //[Header("Items")]
    //[SerializeField] Item stone;
    //[SerializeField] Item wood;
    //[SerializeField] Item food;
    //[SerializeField] Item ore;

    const string saveKey = "items";
    const string resourcesPath = "Items";

    bool isInited;

    InventoryScreen invScreen;

    public InventoryScreen InventoryScreen => invScreen;
    public ItemsInventory ItemsInventory => inventory;

    public override void OnInit()
    {
        Load();

        var configs = Resources.LoadAll<ItemConfig>(resourcesPath);
        inventory.Init(configs);

        isInited = true;

        game.Inventory = this;

        invScreen=FindObjectOfType<InventoryScreen>();

        for (int i = 0; i < inventory.Items.Count; i++)
        {
            invScreen.createItemUI(inventory.Items[i]);
        }
        Signals.Get<TryToPickUpResourceSignal>().AddListener(TryToPickUpResource);
        Signals.Get<ResourcePickedSignal>().AddListener(UpdateUI);
    }

    private void TryToPickUpResource(Item res,PickableComponent pc)
    {
        if (true/*inventory.Count() < game.BackPackMax*/)
        {
            inventory.Add(res);
            pc.FlyTo(game.Player.transform);
        }
    }

    private void UpdateUI(Item res)
    {
       invScreen.UpdateUI(res, inventory.ResCount(res));
        //foreach (var item in inventory.Items)
        //{
        //    invScreen.UpdateUI(item, item.Count);
        //}
    }


    public override void OnUpdate()
    {
       // if (!isInited) return;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }
    public void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            Save();
        }
    }

    private void OnDestroy()
    {
        Save();
    }

    // You don't need to save or load manually when using PlayerData
    private void Save()
    {
        SaveExtension.Save(inventory, saveKey);
    }

    private void Load()
    {
        inventory = SaveExtension.Load(saveKey, new ItemsInventory());
    }
}
