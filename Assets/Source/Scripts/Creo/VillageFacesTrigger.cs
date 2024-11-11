using DG.Tweening;
using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageFacesTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] sadFaces;
    [SerializeField] GameObject[] happyFaces;
    [SerializeField] GameObject workbench;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Bootstrap.Instance.PlayerData.VisualLevel==0)
            {
                workbench.SetActive(true);
                foreach (var go in sadFaces)
                {
                    go.SetActive(true);
                    Sequence scaleAndRotateSequence = DOTween.Sequence();

                    scaleAndRotateSequence.Append(go.transform.DOScale(1.2f, 0.5f));
                    scaleAndRotateSequence.Append(go.transform.DOScale(Vector3.one, 0.5f));
                    scaleAndRotateSequence.SetLoops(-1, LoopType.Restart);
                }

            }
            if (Bootstrap.Instance.PlayerData.VisualLevel == 2)
            {
                foreach (var go in happyFaces)
                {
                    go.SetActive(true);
                    Sequence scaleAndRotateSequence = DOTween.Sequence();

                    scaleAndRotateSequence.Append(go.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f));
                    scaleAndRotateSequence.Append(go.transform.DOScale(Vector3.one, 0.5f));
                    scaleAndRotateSequence.SetLoops(-1, LoopType.Restart);

                }

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var go in sadFaces)
            {
                go.SetActive(false);
            }
            foreach (var go in happyFaces)
            {
                go.SetActive(false);
            }

        }
    }

}
