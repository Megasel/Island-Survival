using DG.Tweening;
using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUIComponent : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI count;
    [SerializeField] string itemId;
    public string ItemId => itemId;


    public void UpdateResCount(float resCount)
    {
        count.text = resCount.ToString();
        if (resCount == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            icon.transform.DOScale(1.3f, 0.2f).OnComplete(() => { icon.transform.DOScale(1f, 0.2f);});
        }
    }

    public void SetItem(Item item)
    {
        itemId = item.Id;
        icon.sprite = item.Config.Icon;
        count.text = item.Count.ToString();
        if (item.Count == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
