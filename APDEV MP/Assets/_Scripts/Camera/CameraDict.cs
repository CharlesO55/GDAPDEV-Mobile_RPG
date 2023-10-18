using System;
using Cinemachine;
using UnityEngine;

[Serializable]
public class CameraDict
{
    [SerializeField] public CinemachineVirtualCamera virtualCam;
    [SerializeField] public EnumCameraID camID;
}