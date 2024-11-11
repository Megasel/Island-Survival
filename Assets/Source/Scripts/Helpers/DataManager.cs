using Kuhpik;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
    }

    public void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveDCObjectData(int objectID, float health, int count)
    {
        if (Bootstrap.Instance.PlayerData.ResourcesDataDictionary.TryGetValue(objectID, out DcData objectData))
        {
            objectData.health = health;
            objectData.count = count;
        }
        else
        {
            DcData newData = new DcData();
            newData.health = health;
            newData.count = count;
            Bootstrap.Instance.PlayerData.ResourcesDataDictionary.Add(objectID, newData);
        }
    }

    public DcData LoadDCObjectData(int objectID)
    {
        if (Bootstrap.Instance.PlayerData.ResourcesDataDictionary.TryGetValue(objectID, out DcData objectData))
        {
            return objectData;
        }
        else
        {
            return null;
        }
    }

    public void SavePCObjectData(int objectID, bool isPicked)
    {
        if (Bootstrap.Instance.PlayerData.PickableResDataDictionary.TryGetValue(objectID, out bool Picked))
        {
            Bootstrap.Instance.PlayerData.PickableResDataDictionary[objectID] = isPicked;
        }
        else
        {
            Bootstrap.Instance.PlayerData.PickableResDataDictionary.Add(objectID, isPicked);
        }
        Bootstrap.Instance.SaveGame();
    }

    public bool LoadPCObjectData(int objectID)
    {
        if (Bootstrap.Instance.PlayerData.PickableResDataDictionary.TryGetValue(objectID, out bool isPicked))
        {
            return isPicked;
        }
        else
        {
            return false;
        }
    }

    public void SaveRCObjectData(int objectID, float count,int ItemIndex)
    {
        if (Bootstrap.Instance.PlayerData.RecyclersDataDictionary.TryGetValue(objectID, out RcData rcData))
        {
            rcData.count= count;
            rcData.index= ItemIndex;
        }
        else
        {
            RcData newData = new RcData();
            newData.index = ItemIndex;
            newData.count = count;
            Bootstrap.Instance.PlayerData.RecyclersDataDictionary.Add(objectID, newData);
        }
        Bootstrap.Instance.SaveGame();
    }

    public RcData LoadRCObjectData(int objectID)
    {
        if (Bootstrap.Instance.PlayerData.RecyclersDataDictionary.TryGetValue(objectID, out RcData rcData))
        {
            return rcData;
        }
        else
        {
            return null;
        }
    }

    public void SaveHelperObjectData(int objectID, bool isHired,bool onShip)
    {
        if (Bootstrap.Instance.PlayerData.HelpersDataDictionary.TryGetValue(objectID, out HelperData HData))
        {
            HData.isHired= isHired;
            HData.onShip= onShip;
        }
        else
        {
            HelperData newData = new HelperData();
            newData.isHired = isHired;
            newData.onShip = onShip;
            Bootstrap.Instance.PlayerData.HelpersDataDictionary.Add(objectID, newData);
        }
        Bootstrap.Instance.SaveGame();
    }

    public HelperData LoadHelperObjectData(int objectID)
    {
        if (Bootstrap.Instance.PlayerData.HelpersDataDictionary.TryGetValue(objectID, out HelperData HData))
        {
            return HData;
        }
        else
        {
            return null;
        }
    }

}
