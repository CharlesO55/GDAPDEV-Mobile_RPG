
using UnityEngine;
using System;
using Cinemachine;
using System.Runtime.CompilerServices;

public class CustomCameraSwitcher : MonoBehaviour
{
    public static CustomCameraSwitcher Instance;

    [SerializeField] CameraDict[] _cameras;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
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
    /*
        private bool TryGetCamera(EcamID camID, out CinemachineVirtualCamera vCam)
        {
            vCam = null;
            foreach (CamDict cam  in _cameras)
            {
                if(cam.camID == camID)
                {
                    vCam = cam.vCam;
                    return true;
                }
            }
            return false;
        }*/
}