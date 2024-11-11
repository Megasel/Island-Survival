using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelperHireUI : MonoBehaviour
{
    [SerializeField] GameObject hirePanelObject;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] Button hireButton;
    [SerializeField] Button closeButton;

    public void SetUI(HumanHelper humanHelper)
    {
        icon.sprite = humanHelper.hireItemCost.Config.Icon;
        costText.text = humanHelper.hireItemCost.Count.ToString();
        hireButton.onClick.RemoveAllListeners();
        hireButton.onClick.AddListener(humanHelper.Hire);
        hireButton.interactable = Bootstrap.Instance.GameData.Inventory.ItemsInventory.IsEnough(humanHelper.hireItemCost);
        hirePanelObject.SetActive(true);
        closeButton.onClick.AddListener(Close);
    }

    public void Close()
    {
        hirePanelObject.SetActive(false);
    }
}
