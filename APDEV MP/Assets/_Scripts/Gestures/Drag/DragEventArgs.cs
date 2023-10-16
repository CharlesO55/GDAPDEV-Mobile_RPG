using UnityEngine;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
public class DragEventArgs
{
    private ETouch.Touch _trackedFinger;
    private GameObject _objHit;

    public ETouch.Touch TrackedFinger
    {
        get { return _trackedFinger; }
    }
    public GameObject ObjHit
    {
        get { return _objHit; }
        set { _objHit = value; }
    }

    public DragEventArgs(ETouch.Touch trackedFinger, GameObject objHit = null)
    {
        _trackedFinger = trackedFinger;
        _objHit = objHit;
    }
}
