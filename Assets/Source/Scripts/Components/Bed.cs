using DG.Tweening;
using Kuhpik;
using Supyrb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bed : MonoBehaviour
{
    [SerializeField] GameObject BedUIGO;
    [SerializeField] Button restButton;
    [SerializeField] Button closeButton;
    [SerializeField] float amountOfEnergyToRestore = 75;
    [SerializeField] Transform bedLayDownStartPosition;
    public Button RestButton=>restButton;
    private void Start()
    {
        restButton.onClick.AddListener(AddEnergy);
        closeButton.onClick.AddListener(() => BedUIGO.SetActive(false));

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Bootstrap.Instance.PlayerData.Energy<75)
            {
                BedUIGO.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BedUIGO.SetActive(false);
        }
    }

    private void AddEnergy()
    {
        var energyToRestore =amountOfEnergyToRestore-Bootstrap.Instance.PlayerData.Energy;
        //Signals.Get<EnergyChangedSignal>().Dispatch(energyToRestore);
        BedUIGO.SetActive(false);
        Signals.Get<PlayerCanMoveSignal>().Dispatch(false);
        Bootstrap.Instance.GameData.Player.GetComponent<Collider>().enabled = false;
        Bootstrap.Instance.GameData.Player.transform.DOMove(bedLayDownStartPosition.position, .5f).OnComplete(RestAnimation);
        Bootstrap.Instance.GameData.Player.transform.DORotate(bedLayDownStartPosition.eulerAngles,.5f);
        DOVirtual.DelayedCall(3f, () => Signals.Get<EnergyChangedSignal>().Dispatch(energyToRestore));
        DOVirtual.DelayedCall(6f,() => { Signals.Get<PlayerCanMoveSignal>().Dispatch(true);
            Bootstrap.Instance.GameData.Player.GetComponent<Collider>().enabled = true;
        });
    }
    private void RestAnimation()
    {
        var animator = Bootstrap.Instance.GameData.Anim;
        animator.SetTrigger("Sleep");
    }

}
