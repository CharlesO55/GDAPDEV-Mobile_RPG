using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraTargetter : MonoBehaviour
{
    private GameObject _targetObject;


    //ROTATION FIELDS
    private float _rotateAmount;
    private EnumDirection _rotDir;
    [SerializeField] private float _rotateSpeed = 150;



    private void Start()
    {
        PartyManager.Instance.OnSwitchPlayerEvent += SetTarget;
        GestureManager.Instance.OnSwipeDelegate += SwipeRotate;
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
        //Only swipe when not interacting with anything
        if (args.ObjHit != null)
        {
            return;
        }

        
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
}
