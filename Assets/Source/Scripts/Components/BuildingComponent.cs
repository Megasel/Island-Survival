using DG.Tweening;
using JetBrains.Annotations;
using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BuildingComponent : MonoBehaviour
{
    [HideInInspector] public BuildingData BuildingData;
    [SerializeField] public BuildingType buildingType;
    [SerializeField] GameObject buildGO;
    [SerializeField] GameObject openGO;
    [SerializeField] BuildingComponent[] nextBuildings;
    [SerializeField] ParticleSystem buildVFX;
    [SerializeField] string eventName;
    [SerializeField] AudioSource aud;
    public void SetState()
    {
        openGO.SetActive(BuildingData.IsOpened&&!BuildingData.IsBuild);
        buildGO.SetActive(BuildingData.IsBuild);
        openGO.GetComponent<BuildingProjection>().UpdateUI();
    }
    private void OnEnable()
    {
        aud.playOnAwake = false;
        aud.loop = false;
    }
    public void Open()
    {
        BuildingData.IsOpened= true;
        openGO.SetActive(true);
    }

    public void Build()
    {
        aud.Play();
        buildVFX.Play();
        BuildingData.IsBuild=true;
        openGO.SetActive(false);
        buildGO.SetActive(true);

        if (nextBuildings!=null&&nextBuildings.Length!=0)
        {
            foreach (var b in nextBuildings)
            {
                b.BuildingData.IsOpened = true;
                b.Open();
            }
        }
        Bootstrap.Instance.SaveGame();
    }
}
