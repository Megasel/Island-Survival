using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
enum PanelType {Upgrades, Tools, Rest}
public class StopTimer : MonoBehaviour
{
    [SerializeField] private TimerBeforeAdsYG timerBeforeAdsYG;
    [SerializeField] private PanelType panelType = new PanelType();
    private void OnEnable()
    {
        if(timerBeforeAdsYG == null)
            timerBeforeAdsYG = FindAnyObjectByType<TimerBeforeAdsYG>();
        switch (panelType)
        {
            case PanelType.Upgrades:
                timerBeforeAdsYG.upgradesPanelActivated = true; break;
            case PanelType.Tools:
                timerBeforeAdsYG.toolsPanelActivated = true; break;
            case PanelType.Rest:
                timerBeforeAdsYG.restPanelActivated = true; break;
        }
    }
    private void OnDisable()
    {
        switch (panelType)
        {
            case PanelType.Upgrades:
                timerBeforeAdsYG.upgradesPanelActivated = false; break;
            case PanelType.Tools:
                timerBeforeAdsYG.toolsPanelActivated = false; break;
            case PanelType.Rest:
                timerBeforeAdsYG.restPanelActivated = false; break;
        }
    }
}
