using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapEventArgs
{
    private Vector2 _position;
    public Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public TapEventArgs(Vector2 position)
    {
        this._position = position;
    }
}
