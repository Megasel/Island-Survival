using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeSystem : GameSystem
{
    List<Instrument> instruments;

    [SerializeField] WorkbenchScreen workbenchScreen;

    [SerializeField] GameObject levelUpUIupgradePrefab;
    public GameObject upgradeUI => levelUpUIupgradePrefab;
    [SerializeField] private AudioSource aud;
    private void OnEnable()
    {
        aud.playOnAwake = false;
        aud.loop = false;
    }
    public override void OnInit()
    {
        instruments = player.Instruments;
        workbenchScreen = FindObjectOfType<WorkbenchScreen>();
        SetListeners();
        Signals.Get<InstrumentUpgradedSignal>().AddListener(ShowVfxLevelUp);
        Signals.Get<WorkbenchUIOpened>().AddListener(SetButtons);
    }

    private void SetButtons(List<Instruments> avaliableInstruments)
    {
        workbenchScreen.HammerButton.gameObject.transform.parent.gameObject.SetActive(false);
        workbenchScreen.AxeButton.gameObject.transform.parent.gameObject.SetActive(false);
        workbenchScreen.PickButton.gameObject.transform.parent.gameObject.SetActive(false);
        workbenchScreen.SpearButton.gameObject.transform.parent.gameObject.SetActive(false);
        foreach (var instrument in avaliableInstruments)
        {
            if (instrument == Instruments.Axe) workbenchScreen.AxeButton.gameObject.transform.parent.gameObject.SetActive(true);
            if (instrument == Instruments.Pick) workbenchScreen.PickButton.gameObject.transform.parent.gameObject.SetActive(true);
            if (instrument == Instruments.Hammer) workbenchScreen.HammerButton.gameObject.transform.parent.gameObject.SetActive(true);
            if (instrument == Instruments.Spear) workbenchScreen.SpearButton.gameObject.transform.parent.gameObject.SetActive(true);
        }

        if (avaliableInstruments.Count==1)
        {
            workbenchScreen.CloseButton.transform.parent.GetComponent<HorizontalLayoutGroup>().spacing = 10;
        }
        else if (avaliableInstruments.Count == 2)
        {
            workbenchScreen.CloseButton.transform.parent.GetComponent<HorizontalLayoutGroup>().spacing = 160;
        }
        else if (avaliableInstruments.Count == 3)
        {
            workbenchScreen.CloseButton.transform.parent.GetComponent<HorizontalLayoutGroup>().spacing = 320;
        }
    }

    private void ShowVfxLevelUp(Instruments instrumentType)
    {
        print("!!!");
        aud.Play();
        var instrument = game.Inventory.ItemsInventory.Items.Find(x => x.Id == instrumentType.ToString());
        var tex = levelUpUIupgradePrefab.transform.GetChild(0).GetComponent<ParticleSystem>().textureSheetAnimation;
        tex.SetSprite(0, instrument.Config.Icon);
        levelUpUIupgradePrefab.SetActive(false);
        levelUpUIupgradePrefab.SetActive(true);
    }
    private void SetButtonInteractability(Button button, Instruments instrumentType)
    {
        Instrument instrument = FindInstrument(instrumentType);

        if (instrument.InstrumentType!=Instruments.Axe&& !instruments.Find(x => x.InstrumentType == Instruments.Axe).IsOpened && game.Inventory.ItemsInventory.IsEnough(instruments.Find(x => x.InstrumentType == Instruments.Axe).Costs[0].RequiredItems))
        {
            button.interactable = false;
            workbenchScreen.CloseButton.gameObject.SetActive(false);
        }
        else
        {
            workbenchScreen.CloseButton.gameObject.SetActive(true);

            if (instrument.InstrumentGrade + 1 < instrument.Costs.Count && instrument.IsOpened)
            {
                button.interactable = game.Inventory.ItemsInventory.IsEnough(instrument.Costs[instrument.InstrumentGrade + 1].RequiredItems);
            }
            else
            {
                button.interactable = game.Inventory.ItemsInventory.IsEnough(instrument.Costs[instrument.InstrumentGrade].RequiredItems);
            }
        }
    }
    public override void OnUpdate()
    {
        if (!player.Instruments.Find(x => x.InstrumentType == Instruments.Hammer).IsOpened)
        {
            workbenchScreen.HammerButton.interactable = game.Inventory.ItemsInventory.IsEnough(instruments.Find(x => x.InstrumentType == Instruments.Hammer).Costs[0].RequiredItems);
        }

        SetButtonInteractability(workbenchScreen.AxeButton,Instruments.Axe);
        SetButtonInteractability(workbenchScreen.PickButton, Instruments.Pick);
        SetButtonInteractability(workbenchScreen.SpearButton, Instruments.Spear);
    }

    Instrument FindInstrument(Instruments InstrumentType)
    {
       return instruments.Find(x => x.InstrumentType == InstrumentType);
    }

    private void SetListeners()
    {
        SetupInstrumentButton(workbenchScreen.AxeButton, Instruments.Axe);
        SetupInstrumentButton(workbenchScreen.PickButton, Instruments.Pick);
        SetupInstrumentButton(workbenchScreen.SpearButton, Instruments.Spear);

        if (!player.Instruments.Find(x => x.InstrumentType == Instruments.Hammer).IsOpened)
        {
            workbenchScreen.HammerButton.onClick.AddListener(() =>
            {
                Buy(Instruments.Hammer);
                workbenchScreen.HammerButton.onClick.RemoveAllListeners();
                workbenchScreen.HammerButton.interactable = false;
            });
            UpdateCost(instruments.Find(x => x.InstrumentType == Instruments.Hammer).Costs[0].RequiredItems, Instruments.Hammer);
        }
        else
        {
            UpdateCost(instruments.Find(x => x.InstrumentType == Instruments.Hammer).Costs[0].RequiredItems, Instruments.Hammer);
            workbenchScreen.HammerButton.interactable = false;
        }

        workbenchScreen.CloseButton.onClick.AddListener(() =>
        {
            game.CameraController.SetCamera(CameraType.DefaultCamera);
            Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        });

    }

    private void SetupInstrumentButton(Button button, Instruments instrumentType)
    {
        if (!player.Instruments.Find(x => x.InstrumentType == instrumentType).IsOpened)
        {
            button.onClick.AddListener(() =>
            {
                Buy(instrumentType);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => Upgrade(instrumentType));
                button.GetComponent<Image>().sprite = workbenchScreen.upgradeSprite;
                button.spriteState = workbenchScreen.upgradeSpriteState;
                if (FindInstrument(instrumentType).InstrumentGrade + 1 < FindInstrument(instrumentType).Costs.Count)
                {
                    UpdateCost(FindInstrument(instrumentType).Costs[FindInstrument(instrumentType).InstrumentGrade + 1].RequiredItems, instrumentType);
                }
                else
                {
                    UpdateCost(FindInstrument(instrumentType).Costs[FindInstrument(instrumentType).InstrumentGrade].RequiredItems, instrumentType);
                }
            });
            UpdateCost(FindInstrument(instrumentType).Costs[FindInstrument(instrumentType).InstrumentGrade].RequiredItems, instrumentType);
        }
        else
        {
            button.GetComponent<Image>().sprite = workbenchScreen.upgradeSprite;
            button.spriteState=workbenchScreen.upgradeSpriteState;

            button.onClick.AddListener(() => Upgrade(instrumentType));
            if (FindInstrument(instrumentType).InstrumentGrade+1< FindInstrument(instrumentType).Costs.Count)
            {
                UpdateCost(FindInstrument(instrumentType).Costs[FindInstrument(instrumentType).InstrumentGrade+1].RequiredItems, instrumentType);
            }
            else
            {
                UpdateCost(FindInstrument(instrumentType).Costs[FindInstrument(instrumentType).InstrumentGrade].RequiredItems, instrumentType);
            }
        }
    }

    public void Buy(Instruments instrumentType)
    {
        var instrument = player.Instruments.Find(x => x.InstrumentType == instrumentType);
        instrument.IsOpened=true;
        game.Inventory.ItemsInventory.Subtract(instrument.Costs[instrument.InstrumentGrade].RequiredItems);
        instrument.InstrumentBought();
        foreach (var res in instrument.Costs[instrument.InstrumentGrade].RequiredItems)
        {
            Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(res, Bootstrap.Instance.GameData.Inventory.ItemsInventory.ResCount(res));
        }
        UpdateCost(instrument.Costs[instrument.InstrumentGrade].RequiredItems, instrumentType);
        game.Anim.SetTrigger("Craft");
        Signals.Get<InstrumentCraftedSignal>().Dispatch(instrumentType,0);
        Bootstrap.Instance.SaveGame();
        game.CameraController.SetCamera(CameraType.DefaultCamera);
        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
    }
    public void Upgrade(Instruments instrumentType)
    {
        //Signals.Get<EnergyChangedSignal>().Dispatch(-energyRequiredForCraft);
        var instrument = player.Instruments.Find(x => x.InstrumentType == instrumentType);
        Bootstrap.Instance.GameData.Inventory.ItemsInventory.Subtract(instrument.Costs[instrument.InstrumentGrade+1].RequiredItems);
        instrument.UpgradeBought();
        foreach (var res in instrument.Costs[instrument.InstrumentGrade].RequiredItems)
        {
            Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(res, Bootstrap.Instance.GameData.Inventory.ItemsInventory.ResCount(res));
        }

        if (FindInstrument(instrumentType).InstrumentGrade+1 < FindInstrument(instrumentType).Costs.Count)
        {
            UpdateCost(FindInstrument(instrumentType).Costs[FindInstrument(instrumentType).InstrumentGrade + 1].RequiredItems, instrumentType);
        }
        else
        {
            UpdateCost(FindInstrument(instrumentType).Costs[FindInstrument(instrumentType).InstrumentGrade].RequiredItems, instrumentType);
        }

        game.Anim.SetTrigger("Craft");
        Signals.Get<InstrumentCraftedSignal>().Dispatch(instrumentType, instrument.InstrumentGrade);
        Bootstrap.Instance.SaveGame();
        game.CameraController.SetCamera(CameraType.DefaultCamera);
        Bootstrap.Instance.ChangeGameState(GameStateID.Game);
        Signals.Get<InstrumentUpgradedSignal>().Dispatch(instrumentType);
    }

    private void UpdateCost(Item[] requiredItems, Instruments instrumentType)
    {
        GameObject go = null;//Button button
        switch (instrumentType)
        {
            case Instruments.Hammer:go = workbenchScreen.HammerButton.gameObject.transform.parent.gameObject;
                break;
            case Instruments.Axe: go = workbenchScreen.AxeButton.gameObject.transform.parent.gameObject;
                break;
            case Instruments.Pick:go = workbenchScreen.PickButton.gameObject.transform.parent.gameObject;
                break;
            case Instruments.Spear:go = workbenchScreen.SpearButton.gameObject.transform.parent.gameObject;
                break;
            default:
                break;
        }

        foreach (Transform c in go.transform.GetChild(0))
        {
            Destroy(c.gameObject);
        }
        for (int i = 0; i < requiredItems.Length; i++)
        {
            var resUI = Instantiate(workbenchScreen.ResNeededUIPrefab, go.transform.GetChild(0)).GetComponent<ResourcesNeededUI>();
            resUI.UpdateUI(requiredItems[i].Config.Icon, requiredItems[i].Count.ToString());
        }

        go.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = instruments.Find(x => x.InstrumentType == instrumentType).Icon;
    }
}
