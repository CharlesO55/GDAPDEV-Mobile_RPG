using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TapProperty
{
    [Tooltip("Taps held exceeding time limit are ignored")]
    [SerializeField] 
    private float _maxTapTime = 0.7f;
    public float MaxTapTime { 
        get { return _maxTapTime; } 
        set { _maxTapTime = value; } 
    }


    [Tooltip("Taps held exceeding finger movement limit are ignored")]
    [SerializeField]
    private float _maxDistanceMoved = 0.1f;
    public float MaxDistanceMoved
    {
        get { return _maxDistanceMoved; }
        set { _maxDistanceMoved = value; }
    }
}