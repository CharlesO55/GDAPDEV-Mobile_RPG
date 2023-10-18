using Cinemachine;
using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    [SerializeField] Cinemachine.CinemachineVirtualCamera _virtualCamera;
    [SerializeField] float _maxZoom = 15;
    [SerializeField] float _minZoom = 5;
    //[SerializeField] Vector3 _defaultOffset = new Vector3(0, 2, -5);

    private Vector3 _zoomOffset;


    private void Start()
    {
        PartyManager.Instance.OnSwitchPlayerEvent += SetTarget;
        GestureManager.Instance.OnSwipeDelegate += SwipeRotate;
        GestureManager.Instance.OnSpreadDelegate += SpreadZoom;

        if(_virtualCamera == null)
        {
            Debug.LogError("CameraTargetter requires Cinemachine Virtual Camera to be set");
        }
        _zoomOffset = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        
    }

    private void Update()
    {
        //Always follow the target's position
        this.transform.position = _targetObject.transform.position;

        this.RotateCamera();
    }



    public void SetTarget(object sender, GameObject target)
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


        this._virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = _zoomOffset;
    }
}
