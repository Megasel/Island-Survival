using DG.Tweening;
using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerScreen : UIScreen
{
    [SerializeField] private RectTransform pointerBase;
    [SerializeField] private RectTransform pointerRotationPart;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Vector2 screenBordersMargin = new Vector2(200, 500);

    private Transform camp;
    private Camera currentCamera;
    public override void Open()
    {
        base.Open();
        currentCamera = Camera.main;
    }
    public void SetCamp(Transform target)
    {
        camp = target;
    }
    public void ShowPointer()
    {
        canvasGroup.DOFade(1, 0.3f);
    }
    public void HidePointer()
    {
        canvasGroup.DOFade(0, 0.3f);
    }
    private void LateUpdate()
    {
        if (camp != null)
        {
            RotatePointerToTarget(ref pointerBase, ref pointerRotationPart, camp);
            MovePointer(ref pointerBase, camp);
        }
    }
    private void RotatePointerToTarget(ref RectTransform pBase, ref RectTransform pPart, Transform t)
    {
        //var targetScreenPos = currentCamera.WorldToScreenPoint(t.position);
        var targetScreenPos = GetScreenPosition();
        if (IsTargetOffScreen(targetScreenPos))
        {
            Vector2 dir = (targetScreenPos - pBase.position);
            pPart.rotation = Quaternion.AngleAxis(GetAngleFromVector(dir.normalized) + 90, Vector3.forward);
        }
        else
        {
            pPart.eulerAngles = Vector3.zero;
        }
    }
    private void MovePointer(ref RectTransform pBase, Transform t)
    {
        Vector3 targetScreenPos = GetScreenPosition();
        //var targetScreenPos = currentCamera.WorldToScreenPoint(t.position);
        pBase.transform.position = targetScreenPos;
        if (IsTargetOffScreen(targetScreenPos))
            pBase.transform.position = CapPosition(pBase.transform.position);
    }

    private Vector3 GetScreenPosition()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(currentCamera);
        var playerPos = Bootstrap.Instance.GameData.Player.transform.position;
        Vector3 toCamp = camp.transform.position - playerPos;
        Ray ray = new Ray(playerPos, toCamp);
        //Debug.DrawRay(playerPos, toCamp);
        float rayMinDistance = Mathf.Infinity;
        int index = 0;
        for (int p = 0; p < 4; p++)
        {
            if (planes[p].Raycast(ray, out float distance))
            {
                if (distance < rayMinDistance)
                {
                    rayMinDistance = distance;
                    index = p;
                }
            }
        }
        rayMinDistance = Mathf.Clamp(rayMinDistance, 0, toCamp.magnitude);
        Vector3 worldPosition = ray.GetPoint(rayMinDistance);
        var targetScreenPos = currentCamera.WorldToScreenPoint(worldPosition);
        return targetScreenPos;
    }

    private bool IsTargetOffScreen(Vector2 position)
    {
        return position.x <= screenBordersMargin.x ||
               position.x >= Screen.width - screenBordersMargin.x ||
               position.y <= screenBordersMargin.y ||
               position.y >= Screen.height - screenBordersMargin.y;
    }
    private Vector2 CapPosition(Vector2 position)
    {
        if (position.x <= screenBordersMargin.x) position.x = screenBordersMargin.x;
        if (position.x >= Screen.width - screenBordersMargin.x) position.x = Screen.width - screenBordersMargin.x;
        if (position.y <= screenBordersMargin.y) position.y = screenBordersMargin.y;
        if (position.y >= Screen.height - screenBordersMargin.y) position.y = Screen.height - screenBordersMargin.y;
        return position;
    }
    private float GetAngleFromVector(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

}
