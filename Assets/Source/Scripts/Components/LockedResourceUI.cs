using DG.Tweening;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockedResourceUI : MonoBehaviour
{
    [SerializeField] GameObject lockedResGO;
    [SerializeField] TextMeshProUGUI instrumentNeedText;
    [SerializeField] Image instrumentIcon;

    private void Start()
    {
        Signals.Get<ResourceLockedSignal>().AddListener(UpdateUI);
    }

    private void UpdateUI(Instrument instrument)
    {
        if (instrument == null)
        {
            lockedResGO.transform.DOScale(0, 0.2f).OnComplete(()=>lockedResGO.SetActive(false));
        }
        else
        {
            instrumentIcon.sprite = instrument.Icon;
            instrumentNeedText.text = TutorialStepsLocalization.GetLocalizedText("Upgrade your ") + TutorialStepsLocalization.GetLocalizedText(instrument.InstrumentType.ToString());
            if (!lockedResGO.activeInHierarchy)
            {
                lockedResGO.SetActive(true);
                lockedResGO.transform.DOScale(1, 0.4f);
            }
        }
    }
}
