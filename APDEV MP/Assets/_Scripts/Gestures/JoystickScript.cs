using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickScript : MonoBehaviour, ITappable, IDraggable
{
    [SerializeField] private GameObject _joystickHandle;

    //For recentering the stick
    [SerializeField] private float _recenterSpeed = 500;
    [SerializeField] private float _timeToRecenter = 0.1f; 
    private float _timeSinceLastTouch = 0;

    private RectTransform _rectTransform;
    

    void Start()
    {
        this._rectTransform = this.GetComponent<RectTransform>();

        this._rectTransform.position = new Vector2(Screen.width * .1f, Screen.height * .2f);
    }


    public void OnTapInterface(TapEventArgs args)
    {
        this.SetJoystickPos(args.Position);
    }

    public void OnDragInterface(DragEventArgs args)
    {
        
        this.SetJoystickPos(args.TouchPosition);
        this._timeSinceLastTouch = this._timeToRecenter;
    }

    private void Update()
    {
        if (!DebugginButton.isPaused)
        {
                
           
            this._timeSinceLastTouch -= Time.deltaTime;

            if (_timeSinceLastTouch < 0 && _joystickHandle.transform.localPosition != Vector3.zero)
            {
                this._joystickHandle.transform.localPosition = Vector2.MoveTowards(
                    this._joystickHandle.transform.localPosition,
                    Vector2.zero,
                    this._recenterSpeed * Time.deltaTime);

            }
        }
        

    }


    private void SetJoystickPos(Vector2 position)
    {

        if (!DebugginButton.isPaused)
        {
            
            Vector3[] cr = new Vector3[4];
            this._rectTransform.GetWorldCorners(cr);


            position.x = Mathf.Clamp(position.x, cr[0].x, cr[2].x);
            position.y = Mathf.Clamp(position.y, cr[0].y, cr[2].y);

            _joystickHandle.transform.position = position;
            
        }
       
    }
    public Vector2 GetJoystickAxis(bool bNormalized = false)
    {
        Vector2 axis = new Vector2(
            _joystickHandle.transform.localPosition.x / (this._rectTransform.rect.width / 2),
            _joystickHandle.transform.localPosition.y / (this._rectTransform.rect.height / 2)
        );

        if( bNormalized )
        {
            return axis.normalized;
        }

        return axis;
    }
}