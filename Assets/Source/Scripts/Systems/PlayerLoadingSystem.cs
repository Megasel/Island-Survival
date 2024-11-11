using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PlayerLoadingSystem : GameSystem
{
    public override void OnInit()
    {
        if (player.Instruments==null)
        {
            player.Instruments=new List<Instrument>();
            foreach (var instrument in config.Instruments)
            {
                player.Instruments.Add(instrument.Copy());
            }
            Bootstrap.Instance.SaveGame();
        }
        else
        {
            var configs = Resources.LoadAll<ItemConfig>("Items");
            for (int i = 0; i < player.Instruments.Count; i++)
            {
                for (int z = 0; z < player.Instruments[i].Costs.Count; z++)
                {
                    for (int j = 0; j < player.Instruments[i].Costs[z].RequiredItems.Length; j++)
                    {
                        player.Instruments[i].Costs[z].RequiredItems[j].SetConfig(configs.SingleOrDefault(x => x.Id == player.Instruments[i].Costs[z].RequiredItems[j].Id));
                    }
                }

                player.Instruments[i].Icon= config.Instruments[i].Icon;
            }
        }

        if (player.BuildingsData==null)
        {
            player.BuildingsData = new List<BuildingData>();
            for (int i = 0; i < config.Buildings.Count; i++)
            {
                player.BuildingsData.Add(config.Buildings[i].Copy());
            }

            var buildings = FindObjectsOfType<BuildingComponent>();
            for (int i = 0; i < buildings.Length; i++)
            {
                var b= player.BuildingsData.Find(x => x.BuildingType == buildings[i].buildingType);
                buildings[i].BuildingData =b;
                buildings[i].SetState();
            }
            Bootstrap.Instance.SaveGame();
        }
        else
        {
            var buildings = FindObjectsOfType<BuildingComponent>();
            for (int i = 0; i < buildings.Length; i++)
            {
                var b = player.BuildingsData.Find(x => x.BuildingType == buildings[i].buildingType);
                buildings[i].BuildingData = b;

                var configs=Resources.LoadAll<ItemConfig>("Items");
                for (int j = 0; j < buildings[i].BuildingData.resNeed.Length; j++)
                {
                    buildings[i].BuildingData.resNeed[j].SetConfig(configs.SingleOrDefault(x => x.Id == buildings[i].BuildingData.resNeed[j].Id));
                }

                buildings[i].SetState();
            }
        }

        if (player.ResourcesDataDictionary==null)
        {
            player.ResourcesDataDictionary = new Dictionary<int, DcData>();
            FindObjectOfType<DataManager>().CreateInstance();
            var destructibleObjects=FindObjectsOfType<DestructibleComponent>();
            foreach (var dc in destructibleObjects)
            {
                dc.SaveData();
            }
        }
        else
        {
            FindObjectOfType<DataManager>().CreateInstance();
            var destructibleObjects = FindObjectsOfType<DestructibleComponent>();
            foreach (var dc in destructibleObjects)
            {
                dc.LoadData();
            }
        }

        if (player.PickableResDataDictionary==null)
        {
            player.PickableResDataDictionary = new Dictionary<int, bool>();
            var pickableObjects = FindObjectsOfType<PickableComponent>();
            foreach (var po in pickableObjects)
            {
                po.SaveData();
            }
        }
        else
        {
            var pickableObjects = FindObjectsOfType<PickableComponent>();
            foreach (var po in pickableObjects)
            {
                po.LoadData();
            }
        }

        if (player.RecyclersDataDictionary == null)
        {
            player.RecyclersDataDictionary = new Dictionary<int, RcData>();
            var recyclers = FindObjectsOfType<RecyclerComponent>(true);
            foreach (var rc in recyclers)
            {
                rc.SaveData();
            }
        }
        else
        {
            var recyclers = FindObjectsOfType<RecyclerComponent>(true);
            foreach (var rc in recyclers)
            {
                rc.LoadData();
            }
        }

        if (player.HelpersDataDictionary == null)
        {
            player.HelpersDataDictionary = new Dictionary<int, HelperData>();
            var helpers = FindObjectsOfType<HumanHelper>();
            foreach (var h in helpers)
            {
                h.SaveData();
            }
        }
        else
        {
            var helpers = FindObjectsOfType<HumanHelper>();
            foreach (var h in helpers)
            {
                h.LoadData();
            }
        }

        var islandsManager = FindObjectOfType<IslandsManager>();
        islandsManager.CreateInstance();
        islandsManager.ActivateIslandAndDeactivateOthers(player.IslandLevel);

        if (player.isFirstLaunch)
        {
            player.isFirstLaunch= false;
            Bootstrap.Instance.SaveGame();
        }
    }
}
