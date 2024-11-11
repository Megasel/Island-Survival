using Kuhpik;
using NaughtyAttributes;
using Sirenix.OdinSerializer;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Instruments{Hammer,Axe,Pick,Spear}
[Serializable]
public class Instrument
{
    public string Name;
    public int InstrumentGrade;
    public Instruments InstrumentType;
    public Sprite Icon;
    [HideInInspector] public bool IsOpened;
    public List<InstrumentCost> Costs;

    [HideInInspector] public int Level;
    public float Power;
    public float PowerIncreaseStep;
    public int ActionsBeforePowerIncrease;
    [Range(1f, 5f)] public float ActionsPowerIncreaseMultiplier;
    [HideInInspector] public int CurrentActionsCount;
    public float energyRequiredForAction=1;
    public void AddAction()
    {
        CurrentActionsCount++;
        if (CurrentActionsCount>=ActionsBeforePowerIncrease)
        {
            Power += PowerIncreaseStep;
            CurrentActionsCount= 0;
            ActionsBeforePowerIncrease =(int)(ActionsBeforePowerIncrease*ActionsPowerIncreaseMultiplier);
            Level++;
            Signals.Get<InstrumentUpgradedSignal>().Dispatch(InstrumentType);
        }
        Bootstrap.Instance.SaveGame();
    }

    public void InstrumentBought()
    {
        IsOpened = true;
    }

    public void UpgradeBought()
    {
        Power += PowerIncreaseStep;
        InstrumentGrade++;
        Level++;
        Signals.Get<InstrumentUpgradedSignal>().Dispatch(InstrumentType);
    }

    public Instrument Copy()
    {
        var copy= new Instrument();
        copy.Name= Name;
        copy.InstrumentGrade = InstrumentGrade;
        copy.InstrumentType= InstrumentType;
        copy.IsOpened= IsOpened;
        copy.Icon = Icon;
        copy.Costs =new List<InstrumentCost>();

        for (int i = 0; i < Costs.Count; i++)
        {
            InstrumentCost instrumentCost = new InstrumentCost();
            instrumentCost.RequiredItems = new Item[Costs[i].RequiredItems.Length];
            for (int j = 0; j < Costs[i].RequiredItems.Length; j++)
            {
                instrumentCost.RequiredItems[j]=new Item(Costs[i].RequiredItems[j].Config, Costs[i].RequiredItems[j].Count);
            }
            copy.Costs.Add(instrumentCost);
        }

        copy.Level = Level;
        copy.Power = Power;
        copy.PowerIncreaseStep = PowerIncreaseStep;
        copy.ActionsBeforePowerIncrease = ActionsBeforePowerIncrease;
        copy.ActionsPowerIncreaseMultiplier= ActionsPowerIncreaseMultiplier;
        copy.CurrentActionsCount = CurrentActionsCount;
        copy.energyRequiredForAction= energyRequiredForAction;
        return copy;
    }
}

public enum BuildingType {Workplace,Workplace1,Workplace2,Workplace3,Workbench,Raft,Boat,Boat1,Ship,Sawmill,Forgery,
    Warehouse,Bridge,Bonfire,Bed,Bonfire1,Bed1,Bonfire2,Bed2,Workplace4,Bed3,Bed4,Bonfire3,Bonfire4,Boat2,Boat3,Grill}
public enum RecyclerType {Sawmill,Forgery,Food}
public class DcData
{
    public float health;
    public int count;
}

public class RcData
{
    public int index;
    public float count;
}


public class HelperData
{
    public bool isHired;
    public bool onShip;
}

[Serializable]
public class InstrumentCost
{
    public Item[] RequiredItems;
}
public class Helpers : MonoBehaviour
{

}
