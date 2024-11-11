using DG.Tweening;
using Kuhpik;
using NaughtyAttributes;
using Sirenix.OdinSerializer.Utilities;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildingProjection : MonoBehaviour
{
    [SerializeField] BuildingComponent building;
    [SerializeField] bool isInstrumentNeeded;
    [SerializeField][ShowIf("isInstrumentNeeded")] Instruments[] instrumentNeeded;
    float startingTimeBeforeTakeResource=0.2f;
    float TimeBeforeTakeResouce = 0.2f;
    bool canTakeResource = true;
    [SerializeField] GameObject ResNeededUIContainer;
    [SerializeField] GameObject ResNeedUIPrefab;
    [SerializeField] ResourcesNeededUI[] resourcesNeededUI;
    //float energyPerResource = 1f;
    private void Start()
    {
        building=GetComponentInParent<BuildingComponent>();
    }
    private void OnEnable()
    {
        building = GetComponentInParent<BuildingComponent>();

        CreateUI();
        //resourcesNeededUI=ResNeededUIContainer.GetComponentsInChildren<ResourcesNeededUI>();
        UpdateUI();
    }

    private void CreateUI()
    {
        foreach (Transform c in ResNeededUIContainer.transform)
        {
            Destroy(c.gameObject);
        }
        resourcesNeededUI = new ResourcesNeededUI[building.BuildingData.resNeed.Length];
        for (int i = 0; i < building.BuildingData.resNeed.Length; i++)
        {
            var res = Instantiate(ResNeedUIPrefab, ResNeededUIContainer.transform);
            resourcesNeededUI[i] = res.GetComponent<ResourcesNeededUI>();
        }

        if (isInstrumentNeeded)
        {
            for (int i = 0; i < instrumentNeeded.Length; i++)
            {
                var instrument = Instantiate(ResNeedUIPrefab, ResNeededUIContainer.transform).GetComponent<ResourcesNeededUI>();
                if (instrument == null|| Bootstrap.Instance==null) return;
                instrument.UpdateUI(Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == instrumentNeeded[i]).Icon, "");
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TimeBeforeTakeResouce = startingTimeBeforeTakeResource;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Helper"))
        {
            if (other.GetComponent<HumanHelper>().isHired)
            {
                other.GetComponent<HumanHelper>().onShip = true;
                other.GetComponent<HumanHelper>().SaveData();
                other.gameObject.SetActive(false);
                building.BuildingData.resHave.FirstOrDefault(x=>x.Id=="Helper").Add(1);
                UpdateUI();
                var canBuild = true;
                for (int i = 0; i < building.BuildingData.resNeed.Length; i++)
                {
                    if (building.BuildingData.resNeed[i].Count != building.BuildingData.resHave[i].Count)
                    {
                        canBuild = false;
                    }
                }
                if (canBuild)
                {
                    building.Build();
                }

            }
        }

        bool allInstrumentsOpened = instrumentNeeded.All(instrument => Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == instrument).IsOpened);
        if (other.CompareTag("Player") && canTakeResource&&(!isInstrumentNeeded|| allInstrumentsOpened))
        {
            if (!Bootstrap.Instance.GameData.Anim.GetBool("Run"))
            {
                for (int i = 0; i < building.BuildingData.resNeed.Length; i++)
                {
                    if (building.BuildingData.resNeed[i].Count > building.BuildingData.resHave[i].Count)
                    {
                        var res = Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x => x.Id == building.BuildingData.resNeed[i].Id);
                        if (res.Count > 0/*&&Bootstrap.Instance.PlayerData.Energy>energyPerResource*/)
                        {
                            //Signals.Get<EnergyChangedSignal>().Dispatch(-energyPerResource);
                            res.Subtract(1);
                            foreach (var r in building.BuildingData.resNeed)
                            {
                                Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(res, Bootstrap.Instance.GameData.Inventory.ItemsInventory.ResCount(res));
                            }
                            building.BuildingData.resHave[i].Add(1);
                            var go = Instantiate(res.Config.Prefab, other.transform.position+new Vector3(Random.Range(-1f,1f),0,Random.Range(-1f,1f)), Quaternion.identity);
                            go.GetComponent<Collider>().enabled = false;
                            go.transform.DOJump(transform.position, 2, 1, 0.5f).OnComplete(() => { Destroy(go);UpdateUI();});
                            StartCoroutine(resourceCD());
                        }
                    }
                }
                var canBuild = true;
                for (int i = 0; i < building.BuildingData.resNeed.Length; i++)
                {
                    if (building.BuildingData.resNeed[i].Count != building.BuildingData.resHave[i].Count)
                    {
                        canBuild = false;
                    }
                }
                if (canBuild)
                {
                    building.Build();
                }
            }
            Bootstrap.Instance.SaveGame();
        }
    }

    public void UpdateUI()
    {
        if (resourcesNeededUI.Length == 0)
        {
            CreateUI();
        }
        for (int i = 0; i < building.BuildingData.resNeed.Length; i++)
        {
            resourcesNeededUI[i].UpdateUI(building.BuildingData.resNeed[i].Config.Icon, building.BuildingData.resHave[i].Count+"/"+ building.BuildingData.resNeed[i].Count.ToString());
        }
        //+инструменты и помощники
    }

    IEnumerator resourceCD()
    {
        canTakeResource = false;;
        yield return new WaitForSeconds(TimeBeforeTakeResouce);
        TimeBeforeTakeResouce = TimeBeforeTakeResouce - TimeBeforeTakeResouce / 10;
        canTakeResource = true;
    }

}
