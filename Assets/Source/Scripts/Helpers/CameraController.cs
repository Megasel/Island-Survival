using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Kuhpik;
using UnityEngine;

public class CameraController : GameSystem
{
    [SerializeField] private CameraConfiguration[] cameraConfigurations;
    Vector3 offset;
    public override void OnInit()
    {
        game.CameraController = this;
        game.Cam = Camera.main;
        //Set camera to player pos
        offset = cameraConfigurations[0].Camera.transform.localPosition;//-player.position-
        cameraConfigurations[0].Camera.ForceCameraPosition(game.Player.transform.position+offset,Quaternion.Euler(45,0,0));
    }

    public void SetCamera(CameraType cameraType)
    {
        foreach (var cameraConfiguration in cameraConfigurations)
        {
            cameraConfiguration.Camera.Priority = cameraConfiguration.CameraType == cameraType ? 1 : 0;
        }
    }

    public void SetFollowTarget(Transform target)
    {
        cameraConfigurations[0].Camera.Follow=target;
    }
}

[Serializable]
public class CameraConfiguration
{
    [SerializeField] private CameraType cameraType;
    [SerializeField] private CinemachineVirtualCamera camera;

    public CameraType CameraType => cameraType;

    public CinemachineVirtualCamera Camera => camera;
}

public enum CameraType
{
    DefaultCamera,
    WorkbenchCamera,
}