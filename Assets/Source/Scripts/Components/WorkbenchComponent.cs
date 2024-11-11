using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchComponent : MonoBehaviour
{
    [SerializeField] CameraType cameraType;
    [SerializeField] Collider triggerZone;
    [SerializeField] Image FillImage;
    [SerializeField] float timeBeforeOpenUI;
    [SerializeField] List<Instruments> instrumentsAvaliable;
    [SerializeField] bool canUpgrade;
    //[SerializeField] GameObject WorkBenchUI;
    [SerializeField] int maxUpgrade;
    private float time;

    private void Start()
    {
        //UpdateAvaliableInstruments();
        //Signals.Get<InstrumentUpgradedSignal>().AddListener(Close);
    }

    private void UpdateAvaliableInstruments()
    {
        foreach (var i in Bootstrap.Instance.PlayerData.Instruments)
        {
            if (i.InstrumentGrade == maxUpgrade)
            {
                instrumentsAvaliable.Remove(i.InstrumentType);
            }
        }

        if (!canUpgrade)
        {
            for (int i = 0; i < instrumentsAvaliable.Count; i++)
            {
                if (Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == instrumentsAvaliable[i]).IsOpened)
                {
                    instrumentsAvaliable.Remove(instrumentsAvaliable[i]);
                }
            }
            if (instrumentsAvaliable.Count==0)
            {
                triggerZone.enabled = false;
                FillImage.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        time = 0;
        FillImage.fillAmount = time / timeBeforeOpenUI;
    }

    private void OnTriggerExit(Collider other)
    {
        time = 0;
        FillImage.fillAmount = time / timeBeforeOpenUI;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateFillAmount();
        }
    }

    private void UpdateFillAmount()
    {
        if (Bootstrap.Instance.GetCurrentGamestateID() != GameStateID.Game) return;
        if (instrumentsAvaliable.Count == 0) return;
        UpdateAvaliableInstruments();
        if (time < timeBeforeOpenUI)
        {
            time += Time.deltaTime;
            FillImage.fillAmount = time / timeBeforeOpenUI;
        }
        else
        {
            time = 0; FillImage.fillAmount = time / timeBeforeOpenUI;
            Signals.Get <WorkbenchUIOpened>().Dispatch(instrumentsAvaliable);
            Bootstrap.Instance.ChangeGameState(GameStateID.WorkBenchUpgrade);
            Bootstrap.Instance.GameData.CameraController.SetCamera(cameraType);
        } 
    }
}
