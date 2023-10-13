using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEventArgs
{
    private Vector2 _position;
    private GameObject _objHit;

    public Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }
    public GameObject ObjHit
    {
        get { return this._objHit; }
        set { _objHit = value; }
    }

    public TapEventArgs(Vector2 position, GameObject objHit = null)
    {
        this._position = position;
        this._objHit = objHit;
    }
}
