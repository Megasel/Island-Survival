using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerListenerComponent : MonoBehaviour
{
    bool isLookingAtTarget;
    float maxAngle = 140f;


    float powerMultiplier=1f;

    private void Start()
    {
        Signals.Get<PowerBoostSignal>().AddListener(BoostPower);
        Signals.Get<DisablePowerBoostSignal>().AddListener(DisablePowerBoost);
    }

    private void DisablePowerBoost()
    {
        powerMultiplier=1f;
    }

    private void BoostPower()
    {
        powerMultiplier = 1.3f;//+30% instrument power
    }

    void OnCollisionEnter(Collision collision)
    {

    }

    void OnCollisionExit(Collision collision)
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PickableComponent>() != null)
        {
            other.GetComponent<PickableComponent>().TryToPickUp();
        }

        var dc = other.GetComponent<DestructibleComponent>();
        if (dc == null) return;
        if (CompareTag(dc.InstrumentNeededToInteract.ToString()))
        {
            if (other.GetComponent<DestructibleComponent>() != null)
            {
                if (dc != null)
                {
                    var instrument = Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType== dc.InstrumentNeededToInteract);
                    if (instrument!=null)
                    {
                        if (Bootstrap.Instance.GameData.Anim.GetBool(dc.InstrumentNeededToInteract+instrument.InstrumentGrade.ToString()))
                        {

                            Bootstrap.Instance.GameData.Player.ProgressUI.UpdateUI((float)instrument.CurrentActionsCount / instrument.ActionsBeforePowerIncrease, instrument.Icon,instrument.Level,instrument.InstrumentType);
                            Bootstrap.Instance.GameData.Player.ProgressLevelObj.SetActive(true);
                            if (instrument.InstrumentGrade >= dc.Level)
                            {
                                if (Bootstrap.Instance.PlayerData.Energy-instrument.energyRequiredForAction>=0)
                                {
                                    Signals.Get<EnergyChangedSignal>().Dispatch(-instrument.energyRequiredForAction);
                                    dc.Interact(instrument.Power*powerMultiplier);
                                    instrument.AddAction();
                                }
                                else
                                {
                                    Bootstrap.Instance.GameData.Anim.SetBool(dc.InstrumentNeededToInteract + instrument.InstrumentGrade.ToString(), false);
                                }
                            }
                            Bootstrap.Instance.GameData.Player.ProgressUI.UpdateUI((float)instrument.CurrentActionsCount / instrument.ActionsBeforePowerIncrease,instrument.Icon,instrument.Level,instrument.InstrumentType); 
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<DestructibleComponent>() != null)
        {
            var dc = other.GetComponent<DestructibleComponent>();
            Vector3 directionToTarget = other.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
            if (angleToTarget <= maxAngle / 2f)
            {
                isLookingAtTarget= true;
            }
            else
            {
                isLookingAtTarget= false;
            }


            if (dc != null&&isLookingAtTarget)
            {
                var instrument = Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == dc.InstrumentNeededToInteract);
                if (!instrument.IsOpened) return;

                if (Bootstrap.Instance.PlayerData.Energy-instrument.energyRequiredForAction<0)
                {
                    Signals.Get<NotEnoughEnergySignal>().Dispatch();
                }
                else
                {
                    if (instrument.InstrumentGrade < dc.Level)
                    {
                        Signals.Get<ResourceLockedSignal>().Dispatch(instrument);
                    }
                    else
                    {
                        Bootstrap.Instance.GameData.Anim.SetBool(dc.InstrumentNeededToInteract + instrument.InstrumentGrade.ToString(), true);
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Animal"))
        {
            Bootstrap.Instance.GameData.Anim.SetBool("Spear0", false);
            Bootstrap.Instance.GameData.Anim.SetBool("Spear1", false);
            Bootstrap.Instance.GameData.Anim.SetBool("Spear2", false);
        }
        Signals.Get<ResourceLockedSignal>().Dispatch(null);
    }
}
