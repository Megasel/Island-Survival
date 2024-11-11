using DG.Tweening;
using Kuhpik;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RecyclerComponent : MonoBehaviour
{
    [SerializeField] RecyclerType RecyclerType;
    [SerializeField] Item[] itemsFrom;
    [SerializeField] Item[] itemsTo;
    [SerializeField] Item itemFrom;
    [SerializeField] Item itemTo;
    [SerializeField] float timeBetweenTransformations;
    [SerializeField] Transform ResourcesReadyZone;
    [SerializeField] Transform ResourcesStockZone;
    bool isWorking;
    RecyclerScreen recyclerScreen;
    int resTake;
    int resGive;
    [SerializeField] int resChangeRate;
    [SerializeField] GameObject progressUI;
    [SerializeField] ParticleSystem vfxPS;
    [SerializeField] Image resIcon;
    [SerializeField] Image progressImage;
    [SerializeField] TextMeshProUGUI countToGive;
    private int objectID;

    public Item ItemFrom=>itemFrom;

    private void Awake()
    {
        objectID = GetInstanceID();
        resIcon.sprite = itemTo.Config.Icon;
    }

    public void LoadData()
    {
        var index = DataManager.Instance.LoadRCObjectData(objectID).index;
        itemFrom = itemsFrom[index];
        itemTo = itemsTo[index];
        itemFrom.Add(DataManager.Instance.LoadRCObjectData(objectID).count);
        resIcon.sprite = itemTo.Config.Icon;
    }
    public void SaveData()
    {
        int itemIndex = Array.FindIndex(itemsFrom, x => x.Id == itemFrom.Id); 
        DataManager.Instance.SaveRCObjectData(objectID, itemFrom.Count,itemIndex);
    }

    private void Start()
    {
        recyclerScreen=FindObjectOfType<RecyclerScreen>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!isWorking)
        {
            SetItemsFromTo();
            SetUI();
            recyclerScreen.Open();
        }
    }

    private void SetItemsFromTo()
    {
        var inventory = Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items;

        int selectedItemIndex = itemsFrom.Length - 1;

        for (int i = 0; i < itemsFrom.Length; i++)
        {
            if (inventory.Find(x => x.Id == itemsFrom[i].Id).Count >= resChangeRate)
            {
                selectedItemIndex = i;
                break; 
            }
        }
        itemFrom = itemsFrom[selectedItemIndex];
        itemTo = itemsTo[selectedItemIndex];
        resIcon.sprite = itemTo.Config.Icon;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            recyclerScreen.Close();
        }
    }

    private void SetUI()
    {
        recyclerScreen.recycleButton.onClick.RemoveAllListeners();
        recyclerScreen.minusButton.onClick.RemoveAllListeners();
        recyclerScreen.plusButton.onClick.RemoveAllListeners();
        recyclerScreen.closeButton.onClick.RemoveAllListeners();
        
        recyclerScreen.iconResFrom.sprite = itemFrom.Config.Icon;
        recyclerScreen.iconResTo.sprite = itemTo.Config.Icon;
        resTake = (int)(Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x => x.Id == itemFrom.Id).Count / resChangeRate/*/2*/)*resChangeRate;
        recyclerScreen.countTextFrom.text = resTake.ToString();
        resGive = resTake / resChangeRate;
        recyclerScreen.countTextTo.text = resGive.ToString();
        recyclerScreen.minusButton.onClick.AddListener(()=>AddRes(-1));
        recyclerScreen.plusButton.onClick.AddListener(() => AddRes(1));
        recyclerScreen.recycleButton.onClick.AddListener(()=>Refine());
        recyclerScreen.closeButton.onClick.AddListener(recyclerScreen.Close);
        AddRes(0);
    }

    private void Refine()
    {
        recyclerScreen.Close();
        Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x => x.Id == itemFrom.Id).Subtract(resTake);
        Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(itemFrom, Bootstrap.Instance.GameData.Inventory.ItemsInventory.ResCount(itemFrom));
        itemFrom.Add(resGive);
        SaveData();
        for (int i = 0; i < resTake; i++)
        {
            var go = Instantiate(itemFrom.Config.Prefab, Bootstrap.Instance.GameData.Player.transform.position, Quaternion.identity);
            go.GetComponent<Collider>().enabled = false;
            var offset = new Vector3(Random.Range(-1f,1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
            go.transform.DOBlendableMoveBy(Vector3.up * 2+offset, 0.3f).OnComplete(() => {
                go.transform.DOJump(transform.position, 2, 1, 0.2f).OnComplete(() => { Destroy(go); });
            });
            go.transform.DORotate(new Vector3(0,180,0),0.4f);
        }
    }

    private void AddRes(int i)
    {
        resGive += i;
        resTake=resGive*resChangeRate;
        if (resGive==0)
        {
            recyclerScreen.minusButton.interactable = false;
            recyclerScreen.recycleButton.interactable = false;
        }
        else
        {
            recyclerScreen.minusButton.interactable = true;
            recyclerScreen.recycleButton.interactable = true;
        }
        if (resTake+resChangeRate>Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x=>x.Id==itemFrom.Id).Count)
        {
            recyclerScreen.plusButton.interactable = false;
        }
        else
        {
            recyclerScreen.plusButton.interactable = true;
        }
        UpdateTextUI();
    }

    private void UpdateTextUI()
    {
        recyclerScreen.countTextTo.text = resGive.ToString();
        recyclerScreen.countTextFrom.text = resTake.ToString();
    }


    private void Update()
    {
        if (itemFrom.Count>0&&!isWorking)
        {
            StartCoroutine(Recycle());
        }
    }

    IEnumerator Recycle()
    {
        isWorking= true;
        progressUI.SetActive(true);
        var currentAmount = progressImage.fillAmount;
        vfxPS.Play();

        if (RecyclerType==RecyclerType.Sawmill)////
        {
            var saw = GetComponent<SawRotation>();
            if (saw!=null) { saw.StartRotation(); }
        }

        while (itemFrom.Count!=0)
        {
            countToGive.text = itemFrom.Count.ToString();
            currentAmount = 0;
            DOTween.To(() => currentAmount, x => currentAmount = x, 1, timeBetweenTransformations)
                .OnUpdate(() => { progressImage.fillAmount = currentAmount; });
            yield return new WaitForSeconds(timeBetweenTransformations);
            itemFrom.Subtract(1);
            SaveData();
            //itemTo.Add(1);
            Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x => x.Id == itemTo.Id).Add(1);
            Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(itemTo, Bootstrap.Instance.GameData.Inventory.ItemsInventory.ResCount(itemTo));
        }

        vfxPS.Stop();

        if (RecyclerType == RecyclerType.Sawmill)////
        {
            var saw = GetComponent<SawRotation>();
            if (saw != null) { saw.StopRotation(); }
        }

        isWorking = false;
        progressUI.SetActive(false);
    }
}
