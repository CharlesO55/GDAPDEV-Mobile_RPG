using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DieValues
{
    [SerializeField] private int _faceValue;
    public int FaceValue
    {
        get { return _faceValue; }
        set { _faceValue = value; }
    }

    [SerializeField] private Vector3 _faceAngle;
    public Vector3 FaceAngle
    {
        get { return _faceAngle; }
        set { _faceAngle = value; }
    }

}