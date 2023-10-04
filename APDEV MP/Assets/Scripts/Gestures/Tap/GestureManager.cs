using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GestureManager : MonoBehaviour
{
    public static GestureManager Instance;

    //PARAMS
    [SerializeField] private TapProperty _tapPropertyFields;

    //FINGER TRACK COMPUTE
    private List<Touch> _trackedFingers;
    private float _timePressed;
    private List<Vector2> _startPoints;
    private List<Vector2> _endPoints;
    private bool bGestureDetermined;

    //EVENT HANDLER
    public EventHandler<TapEventArgs> OnTapDelegate;

    private void BroadcastTapEvent()
    {
        if (this.OnTapDelegate == null)
        {
            return;
        }

        //STUFF TO PASS
        TapEventArgs args = new TapEventArgs(this._startPoints[0]);


        //[1] FOR DIRECTED TARGETTED TAPS
        Ray ray = Camera.main.ScreenPointToRay(this._startPoints[0]);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            //DID IT HIT SOMETHING
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj != null)
            {
                //DOES IT HAVE A TAPPABLE INTERFACE?
                if(hitObj.TryGetComponent<ITappable>(out ITappable tappableScript)) 
                { 
                    tappableScript.OnTapInterface(args); 
                }
            }
        }

        //[2] TRIGGER ALL SUBSCRIBED FUNCTIONS TO THIS DELEGATE
        this.OnTapDelegate(this, args);
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
                    //this.CheckSlide
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
            Debug.Log("Tap detected");
            this.bGestureDetermined = true;
            this.BroadcastTapEvent();
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
