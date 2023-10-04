using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TapReceiver : MonoBehaviour
{
    //Start() vs OnEnable()
    //Start will subscribe this object to the delegate once only. Disabling and reenabling will not reconnect this object as listener/observer.
    //OnEnable will resubscribe this object to the delegate. BUT must be disabled during project start to avoid interfering with GestureManager.Awake() setup


    //Once unsubscribed from OnTapDelegate, TapReceiver won't be able to register again unless component is destroyed and instantiated new
    void Start()
    {
        GestureManager.Instance.OnTapDelegate += this.OnTap;
    }

    private void OnDisable()
    {
        GestureManager.Instance.OnTapDelegate -= this.OnTap;
    }

    public void OnTap(object sender, TapEventArgs args)
    {
        Debug.Log(this.gameObject.name + " heard Tap");
    }
}
