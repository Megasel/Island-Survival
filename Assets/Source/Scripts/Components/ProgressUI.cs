using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] Image fillImage;
    [SerializeField] Image instrumentIcon;
    [SerializeField] Image arrow;
    [SerializeField] TextMeshProUGUI progressPercentage;
    [SerializeField] float showTime;
    [SerializeField] TextMeshProUGUI currentLvlText;
    [SerializeField] TextMeshProUGUI nextLvlText;
    [SerializeField] TextMeshProUGUI instrumentText;
    private float time=1f;
    public void UpdateUI(float percentage,Sprite icon,int level,Instruments instrument)
    {
        switch (instrument)
        {
            case Instruments.Hammer:
                break;
            case Instruments.Axe: instrumentText.text = "CUTTING";

                break;
            case Instruments.Pick: instrumentText.text = "MINING";
                break;
            case Instruments.Spear: instrumentText.text = "HUNTING";
                break;
            default:
                break;
        }
        
        currentLvlText.text=level.ToString();
        nextLvlText.text = (level + 1).ToString();
        if (icon != instrumentIcon.sprite)
        {
            fillImage.fillAmount = percentage;
            progressPercentage.text = ((int)(percentage * 100)) + "%";
        }
        time = showTime;
        //fillImage.fillAmount= percentage;
        //progressPercentage.text=((int)(percentage*100))+"%";
        instrumentIcon.sprite= icon;
        arrow.transform.DOScale(1, 0.3f).OnComplete(() => { arrow.transform.DOScale(0, 0.2f); }) ;

        var currentAmount = fillImage.fillAmount;
        DOTween.To(() => currentAmount, x => currentAmount = x, percentage, 0.5f)
            .OnUpdate(() => { fillImage.fillAmount = currentAmount; progressPercentage.text = ((int)(currentAmount * 100)) + "%"; });
       
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
