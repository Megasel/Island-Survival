using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialScreen : UIScreen
{
    [SerializeField] TextMeshProUGUI TutorialStepText;
    [SerializeField] GameObject TutorialObjectUI;
    [SerializeField] GameObject FingerObjectUI;
    public void SetText(string StepText)
    {
        TutorialStepText.text = StepText;
    }
    public void Show()
    {
        TutorialObjectUI.SetActive(true);
    }

    public void Hide()
    {
        TutorialObjectUI.SetActive(false);
    }

    public void ShowFinger(Vector3 pos)
    {
        FingerObjectUI.SetActive(true);
        FingerObjectUI.GetComponent<Transform>().position =new Vector3(pos.x,pos.y,0);
    }

    public void HideFinger()
    {
        FingerObjectUI.SetActive(false);
    }

}
