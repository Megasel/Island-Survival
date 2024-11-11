using Kuhpik;
using UnityEngine;
using TMPro;

public class BoostersTimersScreen : MonoBehaviour
{
    [SerializeField] RectTransform PowerTimer;
    [SerializeField] RectTransform SpeedTimer;

    [SerializeField] TextMeshProUGUI PowerTimerText;
    [SerializeField] TextMeshProUGUI SpeedTimerText;

    Vector2 PowerTimerPos;
    Vector2 SpeedTimerPos;

    GameData game;
    public void PreStart(GameData Game)
    {
        game = Game;
        PowerTimerPos = PowerTimer.anchoredPosition;
        SpeedTimerPos = SpeedTimer.anchoredPosition;
    }
    public void Upd()
    {
        game.BoostPowerTimer = Mathf.Max(game.BoostPowerTimer - Time.deltaTime, 0f);
        game.BoostSpeedTimer = Mathf.Max(game.BoostSpeedTimer - Time.deltaTime, 0f);

        PowerTimer.anchoredPosition = Vector2.Lerp(PowerTimer.anchoredPosition, game.BoostPowerTimer > 0 ? PowerTimerPos - Vector2.right * 180f : PowerTimerPos, Time.deltaTime * 5f);
        SpeedTimer.anchoredPosition = Vector2.Lerp(SpeedTimer.anchoredPosition, game.BoostSpeedTimer > 0 ? SpeedTimerPos - Vector2.right * 180f : SpeedTimerPos, Time.deltaTime * 5f);

        PowerTimerText.text = TimeMinSec(game.BoostPowerTimer);
        SpeedTimerText.text = TimeMinSec(game.BoostSpeedTimer);
    }

    public  string TimeMinSec(float Timer, bool msecs = false)
    {
        string Result;
        int timersecs = (int)(Timer % 60);
        int timermins = (int)((Timer - Timer % 60) / 60);
        float timermsecs = Timer % 1;
        string timerminsstring;
        string timersecsstring;

        if (timermins < 10f)
            timerminsstring = "0" + timermins.ToString();
        else timerminsstring = timermins.ToString();
        if (timersecs < 10f)
            timersecsstring = "0" + timersecs.ToString();
        else timersecsstring = timersecs.ToString();
        Result = timerminsstring + ":" + timersecsstring;
        if (msecs)
            Result += ":" + (timermsecs * 100).ToString("F0");
        return Result;
    }
}
