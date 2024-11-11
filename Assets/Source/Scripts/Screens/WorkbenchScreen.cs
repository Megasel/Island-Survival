using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkbenchScreen : UIScreen
{
    public Button AxeButton;
    public Button PickButton;
    public Button CloseButton;
    public Button SpearButton;
    public Button HammerButton;
    public Image CraftImage;
    public GameObject ResNeededUIPrefab;
    public Sprite craftSprite;
    public Sprite upgradeSprite;

    public Sprite normalUpgradeSprite;

    public SpriteState upgradeSpriteState;
}
