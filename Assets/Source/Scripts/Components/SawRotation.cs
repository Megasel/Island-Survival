using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawRotation : MonoBehaviour
{
    [SerializeField] Transform Saw;
    [SerializeField] float fullCircleRotationTime;
    [SerializeField] Vector3 rotationVector;
    private Tween rotationTween;
    public void StartRotation()
    {
        rotationTween = Saw.DOLocalRotate(rotationVector, fullCircleRotationTime, RotateMode.WorldAxisAdd).SetLoops(-1);
    }

    public void StopRotation()
    {
        if (rotationTween != null && rotationTween.IsActive())
            rotationTween.Kill();
    }
}
