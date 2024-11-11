using DG.Tweening;
using Kuhpik;
using Snippets.Settings;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergySystem : GameSystem
{
    [SerializeField] Image energyFillImage;
    [SerializeField] TextMeshProUGUI currentEnergyText;
    [SerializeField] GameObject NoEnergyGO;
    [SerializeField] Image EnergyImagePrefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform targetPoint;

    public override void OnInit()
    {
        Signals.Get<NotEnoughEnergySignal>().AddListener(NoEnergyShow);
        Signals.Get<EnergyChangedSignal>().AddListener(ChangeEnergyAmount);
        SetUI(player.Energy);
    }

    private void SetUI(float energyAmount)
    {
       var fillAmount=energyAmount/100;
       energyFillImage.fillAmount= fillAmount;
        currentEnergyText.text =(fillAmount* 100).ToString();
    }
    private void UpdateUI()
    {
        var fillAmount = player.Energy/100;
        var currentAmount = energyFillImage.fillAmount;
        DOTween.To(() => currentAmount, x => currentAmount = x, fillAmount, 0.5f)
        .OnUpdate(() => { energyFillImage.fillAmount = currentAmount; currentEnergyText.text = ((int)(currentAmount * 100)) + "%"; })
        .OnComplete(() => {
            energyFillImage.fillAmount = fillAmount;
            currentEnergyText.text = (fillAmount * 100) + "%";
        });
    }
    public void ChangeEnergyAmount(float energyAmountToAdd)
    {
        if (energyAmountToAdd>0)
        {
           StartCoroutine(energyRegenEffect(energyAmountToAdd));
        }
        player.Energy+= energyAmountToAdd;
        player.Energy = player.Energy > 100 ? 100 : player.Energy;//>maxEnergy
        UpdateUI();
    }

    IEnumerator energyRegenEffect(float amount)
    {
        amount = amount / 10;
        for (int i = 0; i < amount; i++)
        {
            var go = Instantiate(EnergyImagePrefab, spawnPoint);
            go.transform.DOScale(2,0.15f).OnComplete(()=>go.transform.DOScale(0.2f,0.15f));
            go.transform.DOJump(targetPoint.position, 1, 1, 0.3f).OnComplete(() => Destroy(go));
            yield return new WaitForSeconds(0.5f/amount);
        }
    }

    public void NoEnergyShow()
    {
        if (!NoEnergyGO.activeInHierarchy)
        {
            NoEnergyGO.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1); 
            sequence.OnComplete(() => NoEnergyGO.SetActive(false));
        }
    }
}
