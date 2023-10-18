using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using System.Reflection;

using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using Unity.VisualScripting;

public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance;

    [Header("Finger Visual Marker")]
    [SerializeField] private GameObject _fingerMarkerPrefab;
    [Tooltip("Ensure the holder is within a canvas")] [SerializeField] private Transform _fingerMarkerHolder;
    private GameObject[] _fingerMarkers;

    [Header("Raycast for controls & ignore layers")]
    [SerializeField] private LayerMask _ignoreRaycastLayer;
    [SerializeField] private LayerMask _uiControlsRaycastableLayer;

    //PARAMS
    [SerializeField] private TapProperty _tapPropertyFields;
    [SerializeField] private SwipeProperty _swipePropertyFields;
    [SerializeField] private DragProperty _dragPropertyFields;
    [SerializeField] private SpreadProperty _spreadPropertyFields;


    //FINGER TRACK COMPUTE
    private bool bGestureDetermined;    //PREVENTS GESTURES FROM OVERLAPPING (Tap vs Drag)

    //EVENT HANDLER
    public EventHandler<TapEventArgs> OnTapDelegate;
    public EventHandler<SwipeEventArgs> OnSwipeDelegate;
    public EventHandler<DragEventArgs> OnDragDelegate;
    public EventHandler<SpreadEventArgs> OnSpreadDelegate;

    //FOR TRACKING THE LAST OBJHIT. PREVENTS OVERRIDING WHEN RAYCAST HITS A DIFFERENT OBJECT
    private GameObject _targetObject;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Warning: Another GestureManager exists");
            Destroy(this.gameObject);
            return;
        }


        Instance = this;
        bGestureDetermined = false;

        this._fingerMarkers = new GameObject[15];
    }

    private void OnEnable()
    {   
        ETouch.EnhancedTouchSupport.Enable();

        ETouch.Touch.onFingerDown += FingerDownDelegate;
        ETouch.Touch.onFingerMove += FingerMoveDelegate;
        ETouch.Touch.onFingerUp += FingerUpDelegate;
    }

    void OnDisable()
    {
        ETouch.Touch.onFingerDown -= FingerDownDelegate;
        ETouch.Touch.onFingerMove -= FingerMoveDelegate;
        ETouch.Touch.onFingerUp -= FingerUpDelegate;
        
        ETouch.EnhancedTouchSupport.Disable();
    }


    /******************************************
     *      WHEN CHECKS ARE CONDUCTED:          
     *          | START  | MOVED | END | CAUSES FALLOUT (bGestureDetermined)
     * TAP                          x          x
     * SWIPE                        x          x
     * DRAG                  x                 x
     * PINCH                 x
     * SPREAD                x
     * ****************************************/

    void FingerDownDelegate(ETouch.Finger finger)
    {
        GameObject clone = Instantiate(this._fingerMarkerPrefab, finger.screenPosition, Quaternion.identity, this._fingerMarkerHolder);
        this._fingerMarkers[finger.index] = clone;

        //WIPE THE PREVIOUS TARGET OBJECT WHEREABOUT
        this._targetObject = null;
    }

    void FingerUpDelegate(ETouch.Finger finger)
    {
        Destroy(this._fingerMarkers[finger.index]);
        this._fingerMarkers[finger.index] = null;

        //SINGLE FINGER
        if(ETouch.Touch.activeFingers.Count == 1) 
        {
            this.CheckTap();
            this.CheckSwipe();
        }

    }

    void FingerMoveDelegate(ETouch.Finger finger)
    {
        //SINGLE FINGER
        switch (ETouch.Touch.activeTouches.Count)
        {
            case 1:
                this.CheckDrag();
                break;
            case 2:
                this.CheckSpread();
                break;
        }
        //DUAL FINGER
    }

    private void Update()
    {
        this.bGestureDetermined = false;
    }



    private void CheckTap()
    {
        //IGNORE WHEN DIFFERENT GESTURE WAS DETECTED
        if (this.bGestureDetermined) {
            return;
        }

        ETouch.Touch tracked = ETouch.Touch.activeTouches[0];

        float distanceMoved = Vector2.Distance(tracked.screenPosition, tracked.startScreenPosition);
        double timePassed = tracked.time - tracked.startTime;

        if(distanceMoved < this._tapPropertyFields.MaxDistanceMoved * Screen.dpi &&
           timePassed < this._tapPropertyFields.MaxTapTime)
        {
            this.bGestureDetermined = true;

            this.FireTapEvent(tracked.startScreenPosition);
        }
    }

    private void FireTapEvent(Vector2 position)
    {
        TapEventArgs args = new TapEventArgs(position);

        if(this.TryGetObjHitByRaycast(position, out GameObject hitObj))
        {
            args.ObjHit = hitObj;

            if(hitObj.TryGetComponent<ITappable>(out ITappable interfaceScript))
            {
                interfaceScript.OnTapInterface(args);
            }
        }

        this.OnTapDelegate?.Invoke(this, args);
    }

    private void CheckSwipe()
    {
        //IGNORE WHEN DIFFERENT GESTURE WAS DETECTED
        if (this.bGestureDetermined)
        {
            return;
        }

        ETouch.Touch tracked = ETouch.Touch.activeTouches[0];

        float distanceMoved = Vector2.Distance(tracked.screenPosition, tracked.startScreenPosition);
        double timePassed = tracked.time - tracked.startTime;

        if (distanceMoved > this._swipePropertyFields.MinDragDistance * Screen.dpi &&
           timePassed < this._swipePropertyFields.MaxTapTime)
        {
            this.bGestureDetermined = true;

            Vector2 rawDir = tracked.screenPosition - tracked.startScreenPosition;

            this.FireSwipeEvent(tracked.startScreenPosition, rawDir, this.CalculateDirection(rawDir));
        }
    }

    private void FireSwipeEvent(Vector2 position, Vector2 rawDir, EnumDirection EDir)
    {
        Debug.Log("Swipe event");

        SwipeEventArgs args = new SwipeEventArgs(position, rawDir, EDir);

        if (this.TryGetObjHitByRaycast(position, out GameObject hitObj))
        {
            args.ObjHit = hitObj;

            if (hitObj.TryGetComponent<ISwipeable>(out ISwipeable interfaceScript))
            {
                interfaceScript.OnSwipeInterface(args);
            }
        }
        this.OnSwipeDelegate?.Invoke(this, args);
    }


    private void CheckDrag()
    {
        if (this.bGestureDetermined)
        {
            return;
        }

        ETouch.Touch tracked = ETouch.Touch.activeTouches[0];
        if(tracked.time - tracked.startTime > this._dragPropertyFields.MinPressTime)
        {
            this.bGestureDetermined = true;
            this.FireDragEvent(tracked.screenPosition);
        }
    }
    private void FireDragEvent(Vector2 touchPosition)
    {
        DragEventArgs args = new DragEventArgs(touchPosition);

        //IF NO TARGET WAS SELECTED, TRY FIND A VALID ONE. 
        if(this._targetObject == null)
        {
            if (this.TryGetObjHitByRaycast(touchPosition, out GameObject hitObj))
            {
                args.ObjHit = hitObj;
                //STORE A COPY FOR USE IN NEXT UPDATES
                this._targetObject = hitObj;
            }
        }
        //USE THE STORED COPY. ALLOWS DRAG TO PERSIST WHEN OBJECTS BLOCK THE RAYCAST DURING UPDATE
        else
        {
            args.ObjHit = this._targetObject;
        }

        //CALL THE INTERFACE
        if (args.ObjHit != null && args.ObjHit.TryGetComponent<IDraggable>(out IDraggable interfaceScript))
        {
            interfaceScript.OnDragInterface(args);
        }

        //BROADCAST
        this.OnDragDelegate?.Invoke(this, args);
    }


    private void CheckSpread()
    {
        /*if (this.bGestureDetermined)
        {
            return;
        }*/

        //Get the distance apart
        float currDistanceApart = Vector2.Distance(ETouch.Touch.activeTouches[0].screenPosition, ETouch.Touch.activeTouches[1].screenPosition);
        float prevDistanceApart = Vector2.Distance(
            ETouch.Touch.activeTouches[0].screenPosition - ETouch.Touch.activeTouches[0].delta, 
            ETouch.Touch.activeTouches[1].screenPosition - ETouch.Touch.activeTouches[1].delta);

        float distanceDelta = currDistanceApart - prevDistanceApart;
        if (Mathf.Abs(distanceDelta) > this._spreadPropertyFields.MinDistanceChange * Screen.dpi)
        {
            this.bGestureDetermined = true;

            this.FireSpreadEvent(distanceDelta);
        }
    }

    private void FireSpreadEvent(float distanceDelta)
    {
        Debug.Log("spread event");
        SpreadEventArgs args = new SpreadEventArgs(distanceDelta);

        //Avoids overwriding/spreading wrong objects when midpoint moves
        if (_targetObject == null)
        {
            Vector2 midpoint = GetMidpoint();
            if (this.TryGetObjHitByRaycast(midpoint, out GameObject objHit))
            {
                _targetObject = objHit;
                args.HitObject = objHit;
            }
        }

        if (_targetObject != null && _targetObject.TryGetComponent<ISpreadable>(out ISpreadable interfaceScript))
        {
            interfaceScript.OnSpreadInterface(args);
        }

        this.OnSpreadDelegate?.Invoke(this, args);
    }

    private EnumDirection CalculateDirection(Vector2 rawDir)
    {
        EnumDirection _resultDir;
        if (Mathf.Abs(rawDir.x) > Mathf.Abs(rawDir.y))
        {
            _resultDir = (rawDir.x > 0) ? EnumDirection.RIGHT : EnumDirection.LEFT;
        }
        else
        {
            _resultDir = (rawDir.y > 0) ? EnumDirection.UP : EnumDirection.DOWN;
        }

        return _resultDir;
    }

    private Vector2 GetMidpoint()
    {
        Vector2 midpoint = Vector2.zero;
        int activeFingies = ETouch.Touch.activeTouches.Count;


        for (int i = 0; i < activeFingies; i++)
        {
            midpoint += ETouch.Touch.activeTouches[0].screenPosition;
        }
        midpoint /= activeFingies;

        return midpoint;
    }
    private bool TryGetObjHitByRaycast(Vector2 position, out GameObject objHit)
    {
        /* [1] RAYCAST FIRST CHECKS INTERACTABLE UI ELEMENTS */

        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = position;

        List<RaycastResult> _uiResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, _uiResults);


        foreach (RaycastResult uiElem in _uiResults)
        {
            // OUR UI CONTROLS ARE EXPECTED TO BE ON LAYER 6
            int objLayer = 1 << uiElem.gameObject.layer;
            if (objLayer == this._uiControlsRaycastableLayer)
            {
                objHit = uiElem.gameObject;
                return true;
            }
        }


        /* [2] IF NO UI WAS HIT, CHECK IF OBJECT WAS HIT */
        Ray ray = Camera.main.ScreenPointToRay(position);

        // Raycast ignores Layer 2 "Ignore Raycast"
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~this._ignoreRaycastLayer))
        {
            objHit = hit.collider.gameObject;
            return true;
        }


        objHit = null;
        return false;
    }
}