using System;
using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

namespace Kuhpik
{
    /// <summary>
    /// Used to store player's data. Change it the way you want.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        // Example (I use public fields for data, but u free to use properties\methods etc)
        // [BoxGroup("level")] public int level;
        // [BoxGroup("currency")] public int money;
        public bool isFirstLaunch=true;
        public int IslandLevel;
        public int VisualLevel;
        public float Speed;
        public float Energy;

        public List<Instrument> Instruments;
        public List<BuildingData> BuildingsData;
        public Dictionary<int, DcData> ResourcesDataDictionary;
        public Dictionary<int, bool> PickableResDataDictionary;
        public Dictionary<int, RcData> RecyclersDataDictionary;
        public Dictionary<int,HelperData> HelpersDataDictionary;

        public bool VibroOn=true;
    }
}