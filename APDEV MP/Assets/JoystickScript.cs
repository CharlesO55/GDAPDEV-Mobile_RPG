using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickScript : MonoBehaviour, ITappable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTapInterface(TapEventArgs args)
    {
        Debug.Log("Joystick tapped");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
