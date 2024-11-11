using Kuhpik;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUIScreen : UIScreen
{
    [SerializeField, BoxGroup("Button")] Button settingsButton;
    [SerializeField, BoxGroup("Button")] Button vibroButton;
    [SerializeField, BoxGroup("GameObject")] GameObject settingsBar;

    public Sprite VibroButtonOn;
    public Sprite VibroButtonOff;

    public Button SettingsButton => settingsButton;
    public Button VibroButton => vibroButton;
    public GameObject SettingsBar => settingsBar;
}
