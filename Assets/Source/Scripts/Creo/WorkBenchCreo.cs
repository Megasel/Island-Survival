using Cinemachine;
using DG.Tweening;
using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class WorkBenchCreo : MonoBehaviour
{
    [SerializeField] GameObject UpgradePanel;
    [SerializeField] Image FingerImage;
    [SerializeField] CinemachineVirtualCamera closeUpCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpgradePanel.SetActive(true);
            FingerImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UpgradePanel.SetActive(false);
            FingerImage.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        closeUpCamera.Priority = 10000;
    }

    public void Upgrade()
    {
        FingerImage.gameObject.SetActive(false);
        UpgradePanel.SetActive(false);
        Bootstrap.Instance.PlayerData.VisualLevel = 2;
        Bootstrap.Instance.PlayerData.Instruments.Find(x=>x.InstrumentType==Instruments.Axe).IsOpened= true;
        Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Axe).InstrumentGrade = 1;
        Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Axe).Power = 1;
        Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Axe).ActionsBeforePowerIncrease =35;
        Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Pick).IsOpened = true;
        Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Pick).InstrumentGrade = 1;
        Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Pick).Power = 1.5f;

        Bootstrap.Instance.PlayerData.Energy = 100;
        Bootstrap.Instance.GameData.Player.UpdateVisual(true);
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        FingerImage.transform.position = mousePosition;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            closeUpCamera.Priority = 0;
            FindObjectOfType<CameraController>().SetCamera(CameraType.DefaultCamera);
        }
    }
}
