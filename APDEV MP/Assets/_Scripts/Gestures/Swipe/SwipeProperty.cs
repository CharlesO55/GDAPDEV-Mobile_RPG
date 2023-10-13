using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SwipeProperty
{
    [Tooltip("Remain within swipe time to avoid being ignored")]
    [SerializeField]
    private float _maxTapTime = 2f;
    public float MaxTapTime
    {
        get { return this._maxTapTime; }
        set { this._maxTapTime = value;}
    }


    [Tooltip("Exceed min to be considered swipe")]
    [SerializeField]
    private float _minDragDistance = 0.7f;
    public float MinDragDistance
    {
        get { return this._minDragDistance;}
        set { this._minDragDistance = value; }
    }
}