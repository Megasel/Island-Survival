using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyZone : MonoBehaviour
{
    bool done;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!done)
        {
            done = true;
            Bootstrap.Instance.PlayerData.Energy = 100;
            Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Pick).IsOpened = true;
            Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Pick).InstrumentGrade = 1;
            Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Pick).Power = 3.5f;
            Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Pick).ActionsBeforePowerIncrease = 35;
        }
    }
}

