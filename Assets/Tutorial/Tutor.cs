using Cinemachine;
using DG.Tweening;
using Kuhpik;
using Snippets.Tutorial;
using Supyrb;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using YG;

public class Tutor : GameSystem
{
    public Item[] resourcesNeededToGather;
    public Transform workPlace;
    public Transform Raft;
    public Transform Bed;
    public Transform workPlaceIsland2;
    public CinemachineVirtualCamera VillageVC;
    public CinemachineVirtualCamera BlockRocksVC;
    public Transform FirePlaceIsland5;
    public Transform Sawmill;

    public override void OnInit()
    {
       FindObjectOfType<TutorialSystem>().Begin();
    }

    public void Logger(string message)
    {
        UnityEngine.Debug.Log(message);
    }
}

[Serializable]
public class TutorialGatherResources : TutorialStep
{
    string tutorialStepText = "gather resources";
    public override void OnUpdate()
    {
       if(Bootstrap.Instance.GameData.Inventory.ItemsInventory.IsEnough(GameObject.FindObjectOfType<Tutor>().resourcesNeededToGather)|| GameObject.FindObjectOfType<Tutor>().workPlace.gameObject.GetComponent<BuildingComponent>().BuildingData.IsBuild)
       Complete();
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
    }
}

[Serializable]
public class TutorialBuildWorkplace : TutorialStep
{
    string tutorialStepText = "build a workplace";

    public override void OnUpdate()
    {
        if (GameObject.FindObjectOfType<Tutor>().workPlace.gameObject.GetComponent<BuildingComponent>().BuildingData.IsBuild)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();

        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().workPlace);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialCraftHammer : TutorialStep
{
    string tutorialStepText = "craft a hammer";

    public override void OnUpdate()
    {
        if (Bootstrap.Instance.GetCurrentGamestateID() == GameStateID.WorkBenchUpgrade)
        {
            TutorialArrow.Instance.DisableArrow();
        }
        else
        {
            TutorialArrow.Instance.ShowArrow();
        }


        if (GameObject.FindObjectOfType<WorkbenchScreen>().HammerButton.gameObject.activeInHierarchy&&Bootstrap.Instance.GameData.Inventory.ItemsInventory.IsEnough(Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Hammer).Costs[0].RequiredItems))
        {
            GameObject.FindObjectOfType<WorkbenchScreen>().CloseButton.interactable = false;
            GameObject.FindObjectOfType<TutorialScreen>().ShowFinger(GameObject.FindObjectOfType<WorkbenchScreen>().HammerButton.GetComponent<RectTransform>().position);
        }
        else
        {
            GameObject.FindObjectOfType<TutorialScreen>().HideFinger();
        }

        if (Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Hammer).IsOpened)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();
        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().workPlace);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<WorkbenchScreen>().CloseButton.interactable = true;
        GameObject.FindObjectOfType<TutorialScreen>().HideFinger();
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialBuildRaft : TutorialStep
{
    string tutorialStepText = "build a raft";

    public override void OnUpdate()
    {
        if (GameObject.FindObjectOfType<Tutor>().Raft.gameObject.GetComponent<BuildingComponent>().BuildingData.IsBuild)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();

        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().Raft);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialBuildBed : TutorialStep
{
    string tutorialStepText = "build a bed";
    bool started = false;

    public override void OnUpdate()
    {
        if (Bootstrap.Instance.PlayerData.IslandLevel == 0) return;
        if (GameObject.FindObjectOfType<Tutor>().Bed.gameObject.GetComponent<BuildingComponent>().BuildingData.IsBuild)
        {
            Complete();
        }
        if (!started)
        {
            started= true;
            TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().Bed);
            TutorialArrow.Instance.ShowArrow();
            GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
            GameObject.FindObjectOfType<TutorialScreen>().Show();
        }

    }

    protected override void OnBegin()
    {
        started = false;
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Show();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialRest : TutorialStep
{
    string tutorialStepText = "go rest";

    public override void OnUpdate()
    {

        if (GameObject.FindObjectOfType<Bed>().RestButton.gameObject.activeInHierarchy)
        {
            GameObject.FindObjectOfType<TutorialScreen>().ShowFinger(GameObject.FindObjectOfType<Bed>().RestButton.GetComponent<RectTransform>().position);
        }
        else
        {
            GameObject.FindObjectOfType<TutorialScreen>().HideFinger();
        }
        if (Bootstrap.Instance.PlayerData.Energy==75)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();

        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().Bed);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialBuildWorkplaceIsland2 : TutorialStep
{
    string tutorialStepText = "build a workplace";

    public override void OnUpdate()
    {
        if (GameObject.FindObjectOfType<Tutor>().workPlaceIsland2.gameObject.GetComponent<BuildingComponent>().BuildingData.IsBuild)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();
        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().workPlaceIsland2);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialCraftAxe : TutorialStep
{
    string tutorialStepText = "craft an axe";

    public override void OnUpdate()
    {
        if (Bootstrap.Instance.GetCurrentGamestateID() == GameStateID.WorkBenchUpgrade)
        {
            TutorialArrow.Instance.DisableArrow();
        }
        else
        {
            TutorialArrow.Instance.ShowArrow();
        }


        if (GameObject.FindObjectOfType<WorkbenchScreen>().transform.GetChild(0).gameObject.activeInHierarchy&& Bootstrap.Instance.GameData.Inventory.ItemsInventory.IsEnough(Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Axe).Costs[0].RequiredItems))
        {
            GameObject.FindObjectOfType<TutorialScreen>().ShowFinger(GameObject.FindObjectOfType<WorkbenchScreen>().AxeButton.GetComponent<RectTransform>().position);
        }
        else
        {
            GameObject.FindObjectOfType<TutorialScreen>().HideFinger();
        }

        if (Bootstrap.Instance.PlayerData.Instruments.Find(x => x.InstrumentType == Instruments.Axe).IsOpened)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();
        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().workPlaceIsland2);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().HideFinger();
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialCutTree : TutorialStep
{
    string tutorialStepText = "cut down the tree";

    DestructibleComponent nearestTree;
    float raycastRadius = 10f;

    public void FindNearestTree()
    {
        var playerTransform = Bootstrap.Instance.GameData.Player.transform;
        float nearestDistance = Mathf.Infinity;
        Collider[] colliders = Physics.OverlapSphere(playerTransform.position, raycastRadius);
        foreach (Collider collider in colliders)
        {
            DestructibleComponent tree = collider.GetComponent<DestructibleComponent>();

            if (tree != null && tree.InstrumentNeededToInteract == Instruments.Axe)
            {
                float distanceToTree = Vector3.Distance(playerTransform.position, tree.transform.position);

                if (distanceToTree < nearestDistance)
                {
                    nearestDistance = distanceToTree;
                    nearestTree = tree;
                }
            }
        }

        if (nearestTree != null)
        {
            raycastRadius = 10f;
            TutorialArrow.Instance.SetTarget(nearestTree.transform);
            TutorialArrow.Instance.ShowArrow();
        }
        else
        {
            raycastRadius += 10f;
            FindNearestTree();
        }
    }
    public override void OnUpdate()
    {
        FindNearestTree();
        if (nearestTree.Health<=0 || (Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x => x.Id == "Wood_lvl1").Count >= 15))
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();
        FindNearestTree();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialShowVillage : TutorialStep
{
    bool isShowing=false;
    CinemachineVirtualCamera VC;
    float timeToWatchTarget=3f;
    void ShowVillage()
    {
        isShowing = true;
        Signals.Get<PlayerCanMoveSignal>().Dispatch(false);
        DOVirtual.DelayedCall(2f, () => {
            Signals.Get<PlayerCanMoveSignal>().Dispatch(false);
            VC = GameObject.FindObjectOfType<Tutor>().VillageVC;
            VC.Priority = 10000;
            DOTween.Sequence()
                .AppendInterval(timeToWatchTarget)
                .AppendCallback(() =>
                {
                    VC.Priority = 0;
                    VC = GameObject.FindObjectOfType<Tutor>().BlockRocksVC;
                    VC.Priority = 10000;
                    DOTween.Sequence()
                .AppendInterval(timeToWatchTarget)
                .AppendCallback(() =>
                {
                    VC.Priority = 0;
                    Complete();
                });
                });

        });
    }

    public override void OnUpdate()
    {
        if (Bootstrap.Instance.PlayerData.IslandLevel != 5) return;
        if (isShowing) return;
        ShowVillage();
    }

    protected override void OnBegin()
    {
        isShowing = false;
    }

    protected override void OnComplete()
    {
        Signals.Get<PlayerCanMoveSignal>().Dispatch(true);
    }
}

[Serializable]
public class TutorialBuildSawmill : TutorialStep
{

    string tutorialStepText = "build a sawmill";

    bool started = false;
    public override void OnUpdate()
    {
        if (!started)
        {
            if (GameObject.FindObjectOfType<Tutor>().FirePlaceIsland5.gameObject.GetComponent<BuildingComponent>().BuildingData.IsBuild)
            {
                started = true;
                GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
                GameObject.FindObjectOfType<TutorialScreen>().Show();
                TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().Sawmill);
                TutorialArrow.Instance.ShowArrow();
            }
        }

        if (GameObject.FindObjectOfType<Tutor>().Sawmill.gameObject.GetComponent<BuildingComponent>().BuildingData.IsBuild)
        {
            Complete();
        }
    }

    protected override void OnBegin()
    {
        started = false;
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        TutorialArrow.Instance.DisableArrow();
    }
}

[Serializable]
public class TutorialRecycleWood : TutorialStep
{

    string tutorialStepText = "recycle wood";

    RecyclerScreen recyclerScreen;
    public override void OnUpdate()
    {
        if (recyclerScreen.transform.GetChild(0).gameObject.activeInHierarchy&&Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x=>x.Id== GameObject.FindObjectOfType<Tutor>().Sawmill.gameObject.GetComponentInChildren<RecyclerComponent>().ItemFrom.Id).Count>=2)
        {
            Signals.Get<PlayerCanMoveSignal>().Dispatch(false);
            recyclerScreen.closeButton.gameObject.SetActive(false);
            TutorialArrow.Instance.DisableArrow();
            GameObject.FindObjectOfType<TutorialScreen>().ShowFinger(GameObject.FindObjectOfType<RecyclerScreen>().recycleButton.GetComponent<RectTransform>().position);

        }

        if (GameObject.FindObjectOfType<Tutor>().Sawmill.gameObject.GetComponentInChildren<RecyclerComponent>().ItemFrom.Count>0)
        {
            GameObject.FindObjectOfType<TutorialScreen>().HideFinger();
            Complete();
        }
    }

    protected override void OnBegin()
    {
        
        recyclerScreen=GameObject.FindObjectOfType<RecyclerScreen>();
        GameObject.FindObjectOfType<TutorialScreen>().SetText(TutorialStepsLocalization.GetLocalizedText(tutorialStepText));
        GameObject.FindObjectOfType<TutorialScreen>().Show();
        TutorialArrow.Instance.SetTarget(GameObject.FindObjectOfType<Tutor>().Sawmill);
        TutorialArrow.Instance.ShowArrow();
    }

    protected override void OnComplete()
    {
        GameObject.FindObjectOfType<TutorialScreen>().Hide();
        Signals.Get<PlayerCanMoveSignal>().Dispatch(true);
        recyclerScreen.closeButton.gameObject.SetActive(true);
    }
}
public class TutorialStepsLocalization
{
    public static string GetLocalizedText(string key)
    {
        switch (key)
        {
            case "gather resources":
                if (YandexGame.EnvironmentData.language == "ru") return "собрать ресурсы";
                else if (YandexGame.EnvironmentData.language == "en") return "gather resources";
                else if (YandexGame.EnvironmentData.language == "tr") return "kaynakları topla";
                else if (YandexGame.EnvironmentData.language == "es") return "recolecta recursos";
                else if (YandexGame.EnvironmentData.language == "de") return "Ressourcen sammeln";
                break;

            case "build workplace":
                if (YandexGame.EnvironmentData.language == "ru") return "построить рабочее место";
                else if (YandexGame.EnvironmentData.language == "en") return "build a workplace";
                else if (YandexGame.EnvironmentData.language == "tr") return "bir iş yeri inşa et";
                else if (YandexGame.EnvironmentData.language == "es") return "construir un lugar de trabajo";
                else if (YandexGame.EnvironmentData.language == "de") return "einen Arbeitsplatz bauen";
                break;

            case "craft hammer":
                if (YandexGame.EnvironmentData.language == "ru") return "создать молоток";
                else if (YandexGame.EnvironmentData.language == "en") return "craft a hammer";
                else if (YandexGame.EnvironmentData.language == "tr") return "bir çekiç yap";
                else if (YandexGame.EnvironmentData.language == "es") return "fabricar un martillo";
                else if (YandexGame.EnvironmentData.language == "de") return "einen Hammer herstellen";
                break;

            case "build raft":
                if (YandexGame.EnvironmentData.language == "ru") return "построить плот";
                else if (YandexGame.EnvironmentData.language == "en") return "build a raft";
                else if (YandexGame.EnvironmentData.language == "tr") return "bir sal yap";
                else if (YandexGame.EnvironmentData.language == "es") return "construir una balsa";
                else if (YandexGame.EnvironmentData.language == "de") return "ein Floß bauen";
                break;

            case "build bed":
                if (YandexGame.EnvironmentData.language == "ru") return "построить кровать";
                else if (YandexGame.EnvironmentData.language == "en") return "build a bed";
                else if (YandexGame.EnvironmentData.language == "tr") return "bir yatak yap";
                else if (YandexGame.EnvironmentData.language == "es") return "construir una cama";
                else if (YandexGame.EnvironmentData.language == "de") return "ein Bett bauen";
                break;

            case "go rest":
                if (YandexGame.EnvironmentData.language == "ru") return "отдохнуть";
                else if (YandexGame.EnvironmentData.language == "en") return "go rest";
                else if (YandexGame.EnvironmentData.language == "tr") return "dinlenmeye git";
                else if (YandexGame.EnvironmentData.language == "es") return "ir a descansar";
                else if (YandexGame.EnvironmentData.language == "de") return "geh ruhen";
                break;

            case "build workplace island2":
                if (YandexGame.EnvironmentData.language == "ru") return "построить рабочее место (остров 2)";
                else if (YandexGame.EnvironmentData.language == "en") return "build a workplace (Island 2)";
                else if (YandexGame.EnvironmentData.language == "tr") return "bir iş yeri inşa et (Ada 2)";
                else if (YandexGame.EnvironmentData.language == "es") return "construir un lugar de trabajo (Isla 2)";
                else if (YandexGame.EnvironmentData.language == "de") return "einen Arbeitsplatz bauen (Insel 2)";
                break;

            case "craft axe":
                if (YandexGame.EnvironmentData.language == "ru") return "создать топор";
                else if (YandexGame.EnvironmentData.language == "en") return "craft an axe";
                else if (YandexGame.EnvironmentData.language == "tr") return "bir balta yap";
                else if (YandexGame.EnvironmentData.language == "es") return "fabricar un hacha";
                else if (YandexGame.EnvironmentData.language == "de") return "eine Axt herstellen";
                break;

            case "cut tree":
                if (YandexGame.EnvironmentData.language == "ru") return "срубить дерево";
                else if (YandexGame.EnvironmentData.language == "en") return "cut down the tree";
                else if (YandexGame.EnvironmentData.language == "tr") return "ağacı kes";
                else if (YandexGame.EnvironmentData.language == "es") return "cortar el árbol";
                else if (YandexGame.EnvironmentData.language == "de") return "den Baum fällen";
                break;

            case "show village":
                if (YandexGame.EnvironmentData.language == "ru") return "показать деревню";
                else if (YandexGame.EnvironmentData.language == "en") return "show the village";
                else if (YandexGame.EnvironmentData.language == "tr") return "köyü göster";
                else if (YandexGame.EnvironmentData.language == "es") return "mostrar el pueblo";
                else if (YandexGame.EnvironmentData.language == "de") return "das Dorf zeigen";
                break;

            case "build sawmill":
                if (YandexGame.EnvironmentData.language == "ru") return "построить лесопилку";
                else if (YandexGame.EnvironmentData.language == "en") return "build a sawmill";
                else if (YandexGame.EnvironmentData.language == "tr") return "bir kereste fabrikası inşa et";
                else if (YandexGame.EnvironmentData.language == "es") return "construir un aserradero";
                else if (YandexGame.EnvironmentData.language == "de") return "eine Sägemühle bauen";
                break;

            case "recycle wood":
                if (YandexGame.EnvironmentData.language == "ru") return "переработать древесину";
                else if (YandexGame.EnvironmentData.language == "en") return "recycle wood";
                else if (YandexGame.EnvironmentData.language == "tr") return "ahşabı geri dönüştür";
                else if (YandexGame.EnvironmentData.language == "es") return "reciclar madera";
                else if (YandexGame.EnvironmentData.language == "de") return "Holz recyceln";
                break;
            case "Upgrade your ":
                if (YandexGame.EnvironmentData.language == "ru") return "Улучшите ";
                else if (YandexGame.EnvironmentData.language == "en") return "Upgrade your ";
                else if (YandexGame.EnvironmentData.language == "tr") return "Yükseltin ";
                else if (YandexGame.EnvironmentData.language == "es") return "Mejora tu ";
                else if (YandexGame.EnvironmentData.language == "de") return "Verbessere dein ";
                break;
            case "Hammer":
                if (YandexGame.EnvironmentData.language == "ru") return "Молот";
                else if (YandexGame.EnvironmentData.language == "en") return "Hammer";
                else if (YandexGame.EnvironmentData.language == "tr") return "Çekiç";
                else if (YandexGame.EnvironmentData.language == "es") return "Martillo";
                else if (YandexGame.EnvironmentData.language == "de") return "Hammer";
                break;

            case "Axe":
                if (YandexGame.EnvironmentData.language == "ru") return "Топор";
                else if (YandexGame.EnvironmentData.language == "en") return "Axe";
                else if (YandexGame.EnvironmentData.language == "tr") return "Balta";
                else if (YandexGame.EnvironmentData.language == "es") return "Hacha";
                else if (YandexGame.EnvironmentData.language == "de") return "Axt";
                break;

            case "Pick":
                if (YandexGame.EnvironmentData.language == "ru") return "Кирку";
                else if (YandexGame.EnvironmentData.language == "en") return "Pick";
                else if (YandexGame.EnvironmentData.language == "tr") return "Kazma";
                else if (YandexGame.EnvironmentData.language == "es") return "Pico";
                else if (YandexGame.EnvironmentData.language == "de") return "Spitzhacke";
                break;

            case "Spear":
                if (YandexGame.EnvironmentData.language == "ru") return "Копье";
                else if (YandexGame.EnvironmentData.language == "en") return "Spear";
                else if (YandexGame.EnvironmentData.language == "tr") return "Mızrak";
                else if (YandexGame.EnvironmentData.language == "es") return "Lanza";
                else if (YandexGame.EnvironmentData.language == "de") return "Speer";
                break;


        }
        return "";
    }

    public void SetTutorialText(string stepKey)
    {
        string tutorialStepText = GetLocalizedText(stepKey);
        GameObject.FindObjectOfType<TutorialScreen>().SetText(tutorialStepText);
    }
}
