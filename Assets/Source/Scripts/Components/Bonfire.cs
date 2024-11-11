using DG.Tweening;
using Kuhpik;
using Supyrb;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Bonfire : MonoBehaviour
{
    [SerializeField] GameObject BonfireUIGO;
    [SerializeField] Button cookButton;
    [SerializeField] Button closeButton;
    [SerializeField] float amountOfEnergyToRestore=25;
    BoostsSystem boosts;
    [SerializeField] Transform bonfireSitPosition;
    private void Start()
    {
        boosts = FindObjectOfType<BoostsSystem>();
        cookButton.onClick.AddListener(AddEnergyAndBonuses);
        closeButton.onClick.AddListener(() =>BonfireUIGO.SetActive(false));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x=>x.Id=="Food").Count>0&& Bootstrap.Instance.PlayerData.Energy < 100)
            //{
            //    BonfireUIGO.SetActive(true);
            //}
            if (Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.FirstOrDefault(x => (x.Id == "Food_lvl1"&&x.Count>0)||(x.Id == "Food_lvl2"&&x.Count>0)|| (x.Id == "Food_lvl3"&&x.Count>0) || (x.Id == "Food_lvl4_Dried" && x.Count > 0)) !=null && Bootstrap.Instance.PlayerData.Energy < 100)
            {
                BonfireUIGO.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.FirstOrDefault(x => (x.Id == "Food_lvl1" && x.Count > 0) || (x.Id == "Food_lvl2" && x.Count > 0) || (x.Id == "Food_lvl3" && x.Count > 0) || (x.Id == "Food_lvl4_Dried" && x.Count > 0)) != null && Bootstrap.Instance.PlayerData.Energy < 100)
            {
                cookButton.enabled = true;
            }
            else
            {
                cookButton.enabled = false;
            }
            //if (Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x => x.Id == "Food").Count <1||Bootstrap.Instance.PlayerData.Energy==100)//<cost
            //{
            //   cookButton.enabled= false;
            //}
            //else
            //{
            //    cookButton.enabled = true;
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BonfireUIGO.SetActive(false);
        }
    }

    private void AddEnergyAndBonuses()
    {
        Signals.Get<PlayerCanMoveSignal>().Dispatch(false);
        Bootstrap.Instance.GameData.Player.GetComponent<Collider>().enabled = false;
        Bootstrap.Instance.GameData.Player.transform.DOMove(bonfireSitPosition.position, .5f).OnComplete(RestAnimation);
        Bootstrap.Instance.GameData.Player.transform.DORotate(bonfireSitPosition.eulerAngles, .5f);

        BonfireUIGO.SetActive(false);
        //var item = Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x => x.Id == "Food");
        var item= Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.FirstOrDefault(x => (x.Id == "Food_lvl1"&&x.Count>0) || (x.Id == "Food_lvl2"&&x.Count>0)|| (x.Id == "Food_lvl3" && x.Count > 0)||(x.Id == "Food_lvl4_Dried" && x.Count > 0));
        item.Subtract(1);
        Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(item, item.Count);

        DOVirtual.DelayedCall(2f, () => Signals.Get<EnergyChangedSignal>().Dispatch(amountOfEnergyToRestore));
        DOVirtual.DelayedCall(4f, () => {
            Signals.Get<PlayerCanMoveSignal>().Dispatch(true);
            Bootstrap.Instance.GameData.Player.GetComponent<Collider>().enabled = true;
            boosts.BoostPower();
            boosts.BoostSpeed();
        });
    }

    private void RestAnimation()
    {
        var animator = Bootstrap.Instance.GameData.Anim;
        animator.SetTrigger("Eat");
    }
}
