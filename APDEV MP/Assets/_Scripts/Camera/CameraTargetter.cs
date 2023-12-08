using Cinemachine;
using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetter : MonoBehaviour
{
    private GameObject _targetObject;


    [Header("Rotation")]
    private float _rotateAmount;
    private EnumDirection _rotDir;
    [SerializeField] private float _rotateSpeed = 150;


    [Header("Zoom")]
    [SerializeField] float _zoomSpeeed = 150;
    //[SerializeField] Cinemachine.CinemachineVirtualCamera _activeCamera;
    [SerializeField] float _maxZoom = 15;
    [SerializeField] float _minZoom = 5;

    private Vector3 _zoomOffset;


    private void OnEnable()
    {
        PartyManager.Instance.OnSwitchPlayerEvent += SetTarget;
        GestureManager.Instance.OnSwipeDelegate += SwipeRotate;
        GestureManager.Instance.OnSpreadDelegate += SpreadZoom;



        /*if(_activeCamera == null)
        {
            Debug.LogError("CameraTargetter requires Cinemachine Virtual Camera to be set");
        }
        _zoomOffset = _activeCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        */

        this._zoomOffset = CustomCameraSwitcher.Instance.ActiveCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    private void OnDisable()
    {
        PartyManager.Instance.OnSwitchPlayerEvent -= SetTarget;
        GestureManager.Instance.OnSwipeDelegate -= SwipeRotate;
        GestureManager.Instance.OnSpreadDelegate -= SpreadZoom;
    }

    private void Update()
    {
        //Always follow the target's position
        this.transform.position = _targetObject.transform.position;

        this.RotateCamera();    
    }



    private void SetTarget(object sender, GameObject target)
    {
        this._targetObject = target;

        //Rotate camera immediately to be behind the target
        this._rotDir = EnumDirection.NONE;
        this.transform.rotation = _targetObject.transform.rotation;
    }

    private void SwipeRotate(object sender, SwipeEventArgs args)
    {
        //Change swipe only when not hitting other obj or when
        /*if (args.ObjHit != null)
        {
            return;
        }*/

        
        this._rotateAmount = 90;
        this._rotDir = args.Direction;
    }

    private void RotateCamera()
    {
        float rotAmount;
        switch (this._rotDir)
        {
            case EnumDirection.LEFT:
                rotAmount = Time.deltaTime * -_rotateSpeed;
                break;
            case EnumDirection.RIGHT:
                rotAmount = Time.deltaTime * _rotateSpeed;
                break;
            default:
                return;
        }

        this.transform.eulerAngles += new Vector3(0, rotAmount, 0);

        _rotateAmount -= MathF.Abs(rotAmount);

        //STOP ROTATING
        if (this._rotateAmount <= 0)
        {
            this._rotDir = EnumDirection.NONE;
        }
    }

    private void SpreadZoom(object sender, SpreadEventArgs args)
    {
        //this.ZoomForPerspectiveCamera(args);

        this.ZoomForOrthoCamera(args);
    }


    private void ZoomForOrthoCamera(SpreadEventArgs args)
    {
        //float currZoom = this._activeCamera.m_Lens.OrthographicSize;
        float currZoom = CustomCameraSwitcher.Instance.ActiveCamera.m_Lens.OrthographicSize;

        currZoom = Mathf.Lerp(currZoom, currZoom - args.DistanceChange, Time.deltaTime * _zoomSpeeed);
        
        currZoom = Mathf.Clamp(currZoom, this._minZoom, this._maxZoom);


        CustomCameraSwitcher.Instance.ActiveCamera.m_Lens.OrthographicSize = currZoom;
        //this._activeCamera.m_Lens.OrthographicSize = currZoom;
    }

    private void ZoomForPerspectiveCamera(SpreadEventArgs args)
    {
        Vector3 zoomDir = _zoomOffset.normalized;


        Vector3 currZoom = _zoomOffset;
        //Basically add a Vector one but at the camera's current facing
        if (args.DistanceChange > 0)
        {
            _zoomOffset -= zoomDir * _zoomSpeeed * Time.deltaTime;
        }
        else if (args.DistanceChange < 0)
        {
            _zoomOffset += zoomDir * _zoomSpeeed * Time.deltaTime;
        }

        _zoomOffset = Vector3.Lerp(currZoom, _zoomOffset, Time.deltaTime * _zoomSpeeed);

        //Cap the zoom amount
        if (_zoomOffset.magnitude > _maxZoom)
        {
            _zoomOffset = zoomDir * _maxZoom;
        }
        else if (_zoomOffset.magnitude < _minZoom || _zoomOffset.y <= 0)
        {
            _zoomOffset = zoomDir * _minZoom;
        }


        Debug.Log("Zoom : " + _zoomOffset.magnitude);


        CustomCameraSwitcher.Instance.ActiveCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _zoomOffset;
        //this._activeCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _zoomOffset;
    }
}
