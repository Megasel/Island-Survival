using DG.Tweening;
using Kuhpik;
using Supyrb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerComponent : MonoBehaviour
{
    const string ROTATETANIM = "Rotate";
    public CharacterController Char;

    public GameObject[] VisualLevels;

    public GameObject ProgressLevelObj;
    public ProgressUI ProgressUI;
    [SerializeField] ParticleSystem upgradeVisualPS;

    public void UpdateVisual(bool isNewIsland=false)
    {
        if (isNewIsland)
        {
            StartCoroutine(UpdateVisualCouroutine());
        }
        else
        {
            for (int i = 0; i < VisualLevels.Length; i++)
            {
                VisualLevels[i].SetActive(false);
            }
            if (Bootstrap.Instance.PlayerData.VisualLevel >= VisualLevels.Length)
             Bootstrap.Instance.PlayerData.VisualLevel = VisualLevels.Length-1;/////
            VisualLevels[Bootstrap.Instance.PlayerData.VisualLevel].SetActive(true);
            Bootstrap.Instance.GameData.Anim = VisualLevels[Bootstrap.Instance.PlayerData.VisualLevel].GetComponent<Animator>();
        }
    }

    IEnumerator UpdateVisualCouroutine()
    {
        Bootstrap.Instance.GameData.Anim.SetTrigger(ROTATETANIM);
        yield return new WaitForSeconds(0.5f);
        AnimatorStateInfo animationState;
        do
        {
            animationState = Bootstrap.Instance.GameData.Anim.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        while (animationState.IsName(ROTATETANIM));

        upgradeVisualPS.Play();

        for (int i = 0; i < VisualLevels.Length; i++)
        {
            VisualLevels[i].SetActive(false);
        }

        Signals.Get<PlayerCanMoveSignal>().Dispatch(true);

        if (Bootstrap.Instance.PlayerData.VisualLevel<VisualLevels.Length)
        {
            VisualLevels[Bootstrap.Instance.PlayerData.VisualLevel].SetActive(true);
            Bootstrap.Instance.GameData.Anim = VisualLevels[Bootstrap.Instance.PlayerData.VisualLevel].GetComponent<Animator>();
            Bootstrap.Instance.GameData.Anim.transform.localPosition=Vector3.down;
        }
    }
}
