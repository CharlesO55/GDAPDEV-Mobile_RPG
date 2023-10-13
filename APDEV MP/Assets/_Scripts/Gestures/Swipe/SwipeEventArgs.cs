using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeEventArgs
{
    private Vector2 _startPosition;
    private Vector2 _rawDirection;
    private EnumDirection _direction;

    private GameObject _objHit;


    public SwipeEventArgs(Vector2 startPosition, Vector2 rawDirection, EnumDirection direction = EnumDirection.NONE, GameObject objHit = null)
    {
        _startPosition = startPosition;
        _rawDirection = rawDirection;
        _direction = direction;
        _objHit = objHit;
    }

    public Vector2 StartPosition
    {
        get { return _startPosition; }
    }
    public EnumDirection Direction
    {
        get { return this._direction; }
    }
    public Vector2 RawDirection
    {
        get { return this._rawDirection; }
    }
    public GameObject ObjHit
    {
        get { return this._objHit; }
        set { this._objHit = value; }
    }


}
