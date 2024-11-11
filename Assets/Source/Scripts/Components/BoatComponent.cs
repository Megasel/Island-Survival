using DG.Tweening;
using Kuhpik;
using Supyrb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatComponent : MonoBehaviour
{
    [SerializeField] CameraType cameraType;
    [SerializeField] Collider triggerZone;
    [SerializeField] GameObject triggerUI;
    [SerializeField] Image FillImage;
    [SerializeField] float timeBeforeGo;
    [SerializeField] Transform playerOnBoat;
    [SerializeField] public Transform playerOffBoat;
    public bool IsNewEra;
    private float time;
    private float speed;
    private float travelTime=3f;
    [SerializeField] Transform destination;
    public Transform Destination=> destination;
    [HideInInspector] public float Speed=>speed;
    private void OnTriggerEnter(Collider other)
    {
        time = 0;
        FillImage.fillAmount = time / timeBeforeGo;
    }

    private void OnTriggerExit(Collider other)
    {
        time = 0;
        FillImage.fillAmount = time / timeBeforeGo;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateFillAmount();
        }
    }

    public void SetDestination(Transform Destination)
    {
        destination= Destination;
    }

    public void SetSpeed()
    {
        speed = 2.5f;
    }


    private void UpdateFillAmount()
    {
        if (Bootstrap.Instance.GetCurrentGamestateID() != GameStateID.Game) return;
        if (time < timeBeforeGo)
        {
            time += Time.deltaTime;
            FillImage.fillAmount = time / timeBeforeGo;
        }
        else
        {
            FindObjectOfType<PlayerUpgradeSystem>().upgradeUI.SetActive(false);
            var gameData = Bootstrap.Instance.GameData;
            Signals.Get<PlayerCanMoveSignal>().Dispatch(false);
            triggerZone.enabled = false;
            gameData.currentBoat = this;
            gameData.Player.transform.DORotate(playerOnBoat.eulerAngles, .5f);
            gameData.Anim.SetTrigger("Jump");
            gameData.Player.transform.DOJump(playerOnBoat.position, 1, 1, 1f).OnComplete(() => {
                speed = (transform.position - destination.position).magnitude / travelTime;
                triggerUI.SetActive(false);
                time = 0; FillImage.fillAmount = time / timeBeforeGo;
                Bootstrap.Instance.ChangeGameState(GameStateID.NextIsland);
                gameData.CameraController.SetCamera(cameraType);
            });
            gameData.Player.transform.SetParent(transform.parent);

            IslandsManager.Instance.ActivateIsland(Bootstrap.Instance.PlayerData.IslandLevel+1);
        }
    }

}
