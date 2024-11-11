using DG.Tweening;
using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InstrumentNameVisual
{
    public string name;
    public GameObject visual;
}
public class InstrumentsVisual : MonoBehaviour
{
    //Dictionary<string,GameObject> visuals;
    [SerializeField] List<InstrumentNameVisual> vList;
    Transform hand;

    private void Start()
    {
        Signals.Get<InstrumentCraftedSignal>().AddListener(ShowInstrument);
    }
    public void ShowInstrument(Instruments instrument,int grade)
    {
        Signals.Get<PlayerCanMoveSignal>().Dispatch(false);
        hand = GameObject.Find("Wrist_R").transform;
        var go = vList.Find(x => x.name == instrument.ToString() + grade).visual;
        go.transform.SetParent(hand);
        go.transform.position= hand.transform.position;
        go.SetActive(true);
        DOVirtual.DelayedCall(2f, () =>{Signals.Get<PlayerCanMoveSignal>().Dispatch(true); go.SetActive(false);});
    }
}
