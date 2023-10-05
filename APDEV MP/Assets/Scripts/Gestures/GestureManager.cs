using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance;

    //PARAMS
    [SerializeField] private TapProperty _tapPropertyFields;
    [SerializeField] private SwipeProperty _swipePropertyFields;

    //FINGER TRACK COMPUTE
    private List<Touch> _trackedFingers;
    private float _timePressed;
    private List<Vector2> _startPoints;
    private List<Vector2> _endPoints;
    private bool bGestureDetermined;

    //EVENT HANDLER
    public EventHandler<TapEventArgs> OnTapDelegate;
    public EventHandler<SwipeEventArgs> OnSwipeDelegate;

    private void BroadcastTapEvent()
    {
        //STUFF TO PASS
        TapEventArgs args = new TapEventArgs(this._startPoints[0], null);

        //[1] FOR DIRECTED TARGETTED TAPS
        if (this.TryGetObjHitByRaycast(this._startPoints[0], out GameObject hitObj))
        {       
            if(hitObj.TryGetComponent<ITappable>(out ITappable tappableScript)) 
            { 
                args.ObjHit = hitObj;
                tappableScript.OnTapInterface(args); 
            }
        }

        //[2] TRIGGER ALL SUBSCRIBED FUNCTIONS TO THIS DELEGATE
        this.OnTapDelegate?.Invoke(this, args);
    }

    private void BroadcastSwipeEvent()
    {
        //DETERMINE THE SWIPE DIR
        Vector2 rawDir = this._endPoints[0] - this._startPoints[0];
        EnumDirection swipeDir = this.CalculateDirection(rawDir);

        SwipeEventArgs args = new SwipeEventArgs(this._startPoints[0], rawDir, swipeDir);

        //[1] CALL THE SWIPED OBJECT'S OnSwipeInterface()
        if (this.TryGetObjHitByRaycast(this._startPoints[0], out GameObject objHit))
        {
            if(objHit.TryGetComponent<ISwipeable>(out ISwipeable swipeableScript))
            {
                args.ObjHit = objHit;
                swipeableScript.OnSwipeInterface(args);
            }
        }
        //[2] CALL ALL SUBSCRIBED FUNCS
        this.OnSwipeDelegate?.Invoke(this, args);
    }

    private bool TryGetObjHitByRaycast(Vector2 position, out GameObject objHit)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            objHit = hit.collider.gameObject;
            return true;
        }
        objHit = null;
        return false;
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

    //USING AWAKE() INSTEAD OF START SINCE TAP_RECEIVERS SUBSCRIBE DURING THEIR START() CALL
    void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("Warning: Another GestureManager exists");
            Destroy(this.gameObject);
            return;
        }
        
        
        Instance = this;
        _trackedFingers = new List<Touch>();
        _startPoints = new List<Vector2>();
        _endPoints = new List<Vector2>();
        bGestureDetermined = false;
    }

    void Update()
    {
        //Check if finger on screen
        if(Input.touchCount <= 0)
        {
            return;
        }

        this._trackedFingers.Clear();
        this._trackedFingers.Add(Input.GetTouch(0));

        switch (_trackedFingers[0].phase)
        {
            case TouchPhase.Began:
                this._timePressed = 0f;
                this._startPoints.Add(_trackedFingers[0].position);
                break;


            case TouchPhase.Ended:
                this._endPoints.Add(_trackedFingers[0].position);
                
                if(_trackedFingers.Count == 1)
                {
                    this.CheckTap();
                    this.CheckSwipe();
                }



                this.ClearAllChecks();

                break;
            default:
                this._timePressed += Time.deltaTime;
                break;
        }
    }

    private void CheckTap()
    {
        if (bGestureDetermined) { return; }

        bool bTimeCheck = this._timePressed < _tapPropertyFields.MaxTapTime;
        bool bFingerMoveCheck = (Vector2.Distance(this._startPoints[0], this._endPoints[0]) * Screen.dpi) < this._tapPropertyFields.MaxDistanceMoved;
        
        if (bTimeCheck && bFingerMoveCheck)
        {
            this.bGestureDetermined = true;
            this.BroadcastTapEvent();
        }
    }

    private void CheckSwipe()
    {
        if (bGestureDetermined) { return; }

        bool bTimeCheck = this._timePressed < _swipePropertyFields.MaxTapTime;
        bool bFingerMoveCheck = Vector2.Distance(this._startPoints[0], this._endPoints[0]) > (this._swipePropertyFields.MinDragDistance * Screen.dpi);


        if(bTimeCheck && bFingerMoveCheck)
        {
            this.bGestureDetermined = true;
            this.BroadcastSwipeEvent();
        }
    }


    private void ClearAllChecks()
    {
        this._trackedFingers.Clear();
        this._startPoints.Clear();
        this._endPoints.Clear();
        this.bGestureDetermined = false;
    }
}
