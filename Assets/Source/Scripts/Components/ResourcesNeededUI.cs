using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesNeededUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI cost;

    public void UpdateUI(Sprite resIcon, string resCount)
    {
        icon.sprite = resIcon;
        cost.text= resCount;
        if (cost.text=="")
        {
            cost.gameObject.SetActive(false);
            icon.rectTransform.anchoredPosition= Vector3.zero;
        }
    }
}
