using DG.Tweening;
using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickableComponent : MonoBehaviour
{
    [SerializeField] Collider col;
    public Item item;
    AudioSource aud;
    private int objectID;
    private bool isPicked;
    private bool needSave=false;

    private void Awake()
    {
        objectID = GetInstanceID();
    }
    private void Start()
    {
        if(aud == null)
        {
            aud = gameObject.AddComponent<AudioSource>();
            aud.playOnAwake = false;
            aud.loop = false;
            aud.clip = Bootstrap.Instance.pickupClip;
        }
    }
    public void LoadData()
    {
        isPicked = DataManager.Instance.LoadPCObjectData(objectID);
        if (isPicked)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        DataManager.Instance.SavePCObjectData(objectID, isPicked);
    }

    public void TryToPickUp()
    {
        needSave = true;
        Signals.Get<TryToPickUpResourceSignal>().Dispatch(item,this);
    }

    public void FlyTo(Transform target)
    {
        col.enabled= false;
        transform.DOScale(0.5f, 0.4f);
        print("pickup");
        aud.Play();
        transform.DOJump(target.position,2,1, 0.4f).OnComplete(() =>
        {
           
            isPicked = true;
            if (needSave)
            {
                SaveData();
            }
            Signals.Get<ResourcePickedSignal>().Dispatch(item);
            Destroy(gameObject);
        });
    }
}
