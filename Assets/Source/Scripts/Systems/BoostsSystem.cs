using DG.Tweening;
using Kuhpik;
using Supyrb;
//using static My;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostsSystem : GameSystem
{
    [SerializeField] GameObject Effect;

    BoostersTimersScreen timers;

    BoostType currentBoostType;

    bool speedBoosted;
    bool powerBoosted;

    public override void OnInit()
    {
        timers = FindObjectOfType<BoostersTimersScreen>(true);

        timers.PreStart(game);
    }
    public override void OnUpdate()
    {
        timers.Upd();

        if (game.BoostSpeedTimer <= 0 && speedBoosted)
        {
            speedBoosted = false;
            Signals.Get<DisableSpeedBoostSignal>().Dispatch();
        }

        if (game.BoostPowerTimer <= 0 && powerBoosted)
        {
            powerBoosted = false;
            Signals.Get<DisablePowerBoostSignal>().Dispatch();
        }
    }


    public void GetBoost(bool status)
    {
        if (!status) return;

        if (currentBoostType == BoostType.Power)
            BoostPower();
        else if (currentBoostType == BoostType.Speed)
            BoostSpeed();

        Bootstrap.Instance.SaveGame();
    }

    public void BoostSpeed()
    {
        game.BoostSpeedTimer += config.BoosterSpeedTime;
        speedBoosted = true;
        Signals.Get<SpeedBoostSignal>().Dispatch();
    }

    public void BoostPower()
    {
        game.BoostPowerTimer += config.BoosterPowerTime;
        powerBoosted = true;
        Signals.Get<PowerBoostSignal>().Dispatch();
    }
}

