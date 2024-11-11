using UnityEngine;

public class IslandsManager : MonoBehaviour
{
    [SerializeField] GameObject[] islands;
    public static IslandsManager Instance;

    public void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ActivateIslandAndDeactivateOthers(int islandLevel)
    {
        for (int i = 0; i < islands.Length; i++)
        {
            DeactivateIsland(i);
        }
        if (islandLevel==0)
        {
            ActivateIsland(islandLevel+1);
        }
        ActivateIsland(islandLevel);
    }
    public void ActivateIsland(int islandLevel)
    {
        islands[islandLevel].SetActive(true);
    }

    public void DeactivateIsland(int islandLevel)
    {
        islands[islandLevel].SetActive(false);
    }

}
