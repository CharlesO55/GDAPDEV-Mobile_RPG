using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadEventArgs
{
    float _distanceChange;
    GameObject _hitObject;

    public float DistanceChange
    {
        get { return _distanceChange; }
    }

    public GameObject HitObject
    {
        get { return this._hitObject; }
        set { this._hitObject = value; }
    }

    public SpreadEventArgs(float distanceChange, GameObject hitObject = null)
    {
        _distanceChange = distanceChange;
        _hitObject = hitObject;
    }
}
