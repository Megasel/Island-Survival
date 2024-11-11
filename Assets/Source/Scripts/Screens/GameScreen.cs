using Kuhpik;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameScreen : UIScreen
{
    [SerializeField] Button characterInfoButton;
    [SerializeField] Button closeCharacterInfoButton;
    [SerializeField] GameObject characterInfoPanel;

    private void Start()
    {
        characterInfoButton.onClick.AddListener(ToggleCharacterInfoPanel);
        closeCharacterInfoButton.onClick.AddListener(ToggleCharacterInfoPanel);
    }

    private void ToggleCharacterInfoPanel()
    {
        if (characterInfoPanel.activeSelf)
        {
            characterInfoPanel.SetActive(false);
        }
        else
        {
            characterInfoPanel.SetActive(true);
        }
    }
}
