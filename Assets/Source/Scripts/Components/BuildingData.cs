using System;
using System.Diagnostics;

[Serializable]
public class BuildingData
{
    public string Name;
    public BuildingType BuildingType;
    public bool IsOpened;
    public bool IsBuild;
    public Item[] resNeed;
    public Item[] resHave;

    public BuildingData Copy()
    {
        var copy = new BuildingData();

        copy.BuildingType = BuildingType;
        copy.IsOpened = IsOpened;
        copy.IsBuild = IsBuild;

        copy.resHave = new Item[resNeed.Length];
        copy.resNeed = new Item[resNeed.Length];

        for (int i = 0; i < resNeed.Length; i++)
        {
            copy.resHave[i] = new Item(resNeed[i].Config, 0);
            copy.resNeed[i] = new Item(resNeed[i].Config, resNeed[i].Count);
        }

        return copy;
    }
}
