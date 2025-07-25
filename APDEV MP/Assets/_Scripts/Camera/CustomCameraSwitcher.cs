
using UnityEngine;
using System;
using Cinemachine;
using System.Runtime.CompilerServices;

public class CustomCameraSwitcher : MonoBehaviour
{
    public static CustomCameraSwitcher Instance;

    [SerializeField] CameraDict[] _cameras;
    public CinemachineVirtualCamera ActiveCamera { get; private set; }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            this.ActiveCamera = _cameras[0].virtualCam;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SwitchCamera(EnumCameraID camID , GameObject overrideTargetObject = null)
    {
        foreach(var cam in _cameras)
        {
            if(cam.camID == camID)
            {
                ActiveCamera = cam.virtualCam;

                cam.virtualCam.Priority = 10;
                if(overrideTargetObject != null)
                {
                    cam.virtualCam.LookAt = overrideTargetObject.transform;
                    cam.virtualCam.Follow = overrideTargetObject.transform;
                }
            }
            else
            {
                cam.virtualCam.Priority = 0;
            }
        }
    }
}