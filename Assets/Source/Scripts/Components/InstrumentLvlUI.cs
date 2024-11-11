using Kuhpik;
using Supyrb;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstrumentLvlUI : MonoBehaviour
{
    [SerializeField] Instruments instrumentType;
    [SerializeField] Instrument instrument;
    [SerializeField] Image icon;
    [SerializeField] Image progressBackground;
    [SerializeField] Image progress;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] List<Sprite> progressSprites;

    private void OnEnable()
    {
        instrument = Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == instrumentType);
        SetUI();
    }

    private void Start()
    {
       // instrument=Bootstrap.Instance.PlayerData.Instruments.Find(x=>x.InstrumentType==instrumentType);
        Signals.Get<InstrumentUpgradedSignal>().AddListener(UpdateUI);
    }
    public void SetUI()
    {
        if (!instrument.IsOpened) return;
        lvlText.text="LVL " + instrument.Level.ToString();
        var bgIndex = (instrument.Level / 5)%progressSprites.Count;
        var progressIndex=(bgIndex+1)>=progressSprites.Count? 0 : (bgIndex + 1);
        progressBackground.sprite = progressSprites[bgIndex];
        progress.sprite = progressSprites[progressIndex];
        progress.fillAmount = (float)(instrument.Level % 5)/5;
    }
    public void UpdateUI(Instruments instrumentType)
    {
        if (this.instrumentType == instrumentType)
        {
            SetUI();
        }
    }
}
