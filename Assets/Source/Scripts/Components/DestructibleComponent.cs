using DG.Tweening;
using Kuhpik;
using Supyrb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static My;
using Random = UnityEngine.Random;

public class DestructibleComponent : MonoBehaviour
{
    public Instruments InstrumentNeededToInteract;
    public int Level;
    public List<GameObject> Parts = new List<GameObject>();
    public GameObject resourseToSpawnPrefab;
    [SerializeField]int countOfResourcesToSpawnTotal;
    [SerializeField] float respawnTime = 10;
    [HideInInspector] public float respawnTimer;
    [SerializeField] Collider col;
    [SerializeField] float InterractMagnitude = 3;
    [SerializeField] bool showHealth;
    [SerializeField] GameObject healthGO;
    [SerializeField] Image healthFill;
    [SerializeField] ParticleSystem HitPs;
    [SerializeField] AudioSource aud;
    int countOfResourcesLeftToSpawn;
    public float Health;
    private int objectID;

    float maxHealth;
    bool Touched;
    float StartY;
    float countToSpawn;
    float countToSpawnPerMissingHealth;
    bool isAnimal;
    private void Awake()
    {
        objectID = GetInstanceID();
        isAnimal = GetComponent<Animal>();
    }
   
    public void LoadData()
    {
        DcData objectData = DataManager.Instance.LoadDCObjectData(objectID);
        if (objectData != null)
        {
            Health = objectData.health;
            countOfResourcesLeftToSpawn = objectData.count;
            UpdateVisual(Health);
            if (Health<=0&&GetComponent<Animal>()!=null)
            {
                Destroy(gameObject);
            }
            else
            {
                this.enabled= false;
            }

            if (showHealth)
            {
                healthGO.SetActive(true);
                healthFill.fillAmount = Health/maxHealth;
            }
        }
    }

    public void SaveData()
    {
        DataManager.Instance.SaveDCObjectData(objectID, Health, countOfResourcesLeftToSpawn);
    }
    private void OnEnable()
    {
        aud.playOnAwake = false;
        aud.loop = false;

    }
    private void Start()
    {
       

         
        respawnTimer = respawnTime;
        maxHealth = Health;
        countOfResourcesLeftToSpawn= countOfResourcesToSpawnTotal;
        StartY = transform.eulerAngles.y;
        countToSpawnPerMissingHealth =countOfResourcesToSpawnTotal / maxHealth;
    }
    public void Respawn()
    {
        foreach (var item in Parts) item.SetActive(true);
        respawnTimer = respawnTime;
        Health = maxHealth;
        countOfResourcesLeftToSpawn = countOfResourcesToSpawnTotal;
        transform.localScale = Vector3.zero;
        transform.DOScale(1.1f, 1f).OnComplete(() => transform.DOScale(1, 0.2f).OnComplete(() => col.enabled = true));
    }

    public void Interact(float Power)
    {
        if (Touched) return;
        Touched = true;


        float healthAfter = Health - Power;

        Health -= Power;

        StartCoroutine(Interacting());
        if (HitPs!=null)
        {
            HitPs.Play();
        }
        UpdateVisual(healthAfter);

        var count = 0;
        if (isAnimal)
        {
            var fillAmount = Health / maxHealth;
            var currentAmount = healthFill.fillAmount;
            DOTween.To(() => currentAmount, x => currentAmount = x, fillAmount, 0.5f)
            .OnUpdate(() => { healthFill.fillAmount = currentAmount; }).OnComplete(() =>
            {
                if (Health<=0&&healthGO!=null)
                {
                    healthGO.SetActive(false);
                } 
            }).SetId(this.gameObject);

            if (Health<=0)
            {
                count = countOfResourcesToSpawnTotal;
                countOfResourcesLeftToSpawn -= count;
            }
        }
        else
        {
            count = (int)(countToSpawnPerMissingHealth * Power);
            if (count == 0)
            {
                countToSpawn += countToSpawnPerMissingHealth * Power;
                if (countToSpawn >= 1)
                {
                    count = (int)countToSpawn;
                    countToSpawn -= count;
                }
            }
            count = count > countOfResourcesLeftToSpawn ? countOfResourcesLeftToSpawn : count;

            var partsActive = 0;

            for (int z = 0; z < Parts.Count; z++)
            {
                if (Parts[z].activeSelf)
                {
                    partsActive++;
                }
            }
            partsActive = partsActive == 0 ? 1 : partsActive;
            if (partsActive == 1)
            {
                count = count < countOfResourcesLeftToSpawn ? countOfResourcesLeftToSpawn : count;
            }

            countOfResourcesLeftToSpawn -= count;
        }

        for (int j = 0; j < count; j++)
        {
            var randomVector = new Vector3(Random.Range(-1.5f, 1.5f), 3, Random.Range(-1.5f, 1.5f));
            var res = Instantiate(resourseToSpawnPrefab, transform.position, Quaternion.identity);
            res.GetComponent<Collider>().enabled = false;
            res.transform.DOJump(transform.position + randomVector, 2, 1, 1f);
            res.transform.DORotate(RandomVector3(-180, 180, -180, 180, -180, 180), 1f).OnComplete(() =>
            {
                res.GetComponent<PickableComponent>().FlyTo(Bootstrap.Instance.GameData.Player.transform);
            });
            Bootstrap.Instance.GameData.Inventory.ItemsInventory.Add(res.GetComponent<PickableComponent>().item);
        }
        SaveData();

        if (isAnimal&&Health<=0)
        {
            DOTween.Kill(this.gameObject);

            Destroy(gameObject,0.1f);
        }
    }

    private void UpdateVisual(float healthAfter)
    {
        for (float i = 1; i < Parts.Count; i++)
        {
            if (i / (float)Parts.Count > healthAfter / maxHealth)
            {
                Parts[(int)i].SetActive(false);
            }
        }
        if (healthAfter <= 0)
        {
            Parts[1].SetActive(false);
            col.enabled = false;
            if (Bootstrap.Instance.GameData.Anim != null)
            {
                Bootstrap.Instance.GameData.Anim.SetBool("Pick0", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Axe0", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Spear0", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Spear1", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Pick1", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Axe1", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Spear2", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Pick2", false);
                Bootstrap.Instance.GameData.Anim.SetBool("Axe2", false);

            }

            //StartCoroutine(RespawnObject());
        }
        else Parts[1].SetActive(true);
    }

    IEnumerator RespawnObject()
    {
        yield return new WaitForSeconds(respawnTimer);
        Respawn();
    }

    IEnumerator Interacting()
    {
        aud.Play();
        Vector3 RandomVec;
        for (float i = 2.0f; i >= 0; i -= 0.5f)
        {
            RandomVec = RandomVector3(-1, 1, 0, 0, -1, 1).normalized * InterractMagnitude * i;
            RandomVec.y = StartY;
            transform.DORotate(RandomVec, 0.1f);
            yield return new WaitForSeconds(0.1f);
            if (i == 1)
                Touched = false;
        }

    }
}
