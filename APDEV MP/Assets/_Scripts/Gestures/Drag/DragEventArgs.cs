using UnityEngine;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
public class DragEventArgs
{
    private Vector2 _touchPosition;
    private GameObject _objHit;


    public Vector2 TouchPosition
    {
        get { return _touchPosition; }
    }
    public GameObject ObjHit
    {
        get { return _objHit; }
        set { _objHit = value; }
    }

    public DragEventArgs(Vector2 touchPosition, GameObject objHit = null)
    {
        _touchPosition = touchPosition;
        _objHit = objHit;
    }
}
