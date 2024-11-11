using DG.Tweening;
using Kuhpik;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsLoadingSystem : GameSystemWithScreen<SettingsUIScreen>
{
    bool isHide = true;
    float time = 0.25f;
    public override void OnInit()
    {
        screen.SettingsButton.onClick.AddListener(() => SettingsHide());
        screen.VibroButton.onClick.AddListener(() => SetVibro());
        Set();
    }
    void SettingsHide()
    {
        screen.SettingsBar.transform.DORewind();

        if (isHide)
        {
            screen.SettingsBar.SetActive(isHide);
            screen.SettingsBar.transform.DOScaleY(1f, time);
        }
        else
        {
            screen.SettingsBar.transform.DOScaleY(0f, time)
                .OnComplete(() => screen.SettingsBar.SetActive(isHide));
        }

        isHide = !isHide;
    }

    void SetVibro()
    {
        player.VibroOn = !player.VibroOn;
        Set();
    }

    void Set()
    {
        if (screen.VibroButton)
            screen.VibroButton.GetComponent<Image>().sprite = player.VibroOn ? screen.VibroButtonOn : screen.VibroButtonOff;

        Bootstrap.Instance.SaveGame();
    }

}