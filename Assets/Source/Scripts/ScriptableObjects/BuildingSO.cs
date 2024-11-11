using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Building")]
public class BuildingSO : ScriptableObject
{
    public bool isOpened;
    public bool isBuild;
    public Item[] itemsRequiredToBuild;
    public Item[] itemsInBuilding;
    public BuildingSO[] buildingsNeededToOpen;

    public bool canOpen => TryOpen();

    private bool TryOpen()
    {
        var canOpen = true;
        for (int i = 0; i < buildingsNeededToOpen.Length; i++)
        {
            if (!buildingsNeededToOpen[i].isOpened)
                canOpen = false;
        }
        return canOpen;
    }
}
