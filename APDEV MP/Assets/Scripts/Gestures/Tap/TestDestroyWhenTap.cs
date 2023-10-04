using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDestroyWhenTap : MonoBehaviour , ITappable
{
    
    // Update is called once per frame
    void Update()
    {
        //this.transform.localScale = transform.localScale + Vector3.one;
    }

    public void OnTapInterface(TapEventArgs args)
    {
        Debug.Log("OH I GOT TAPPED");
        Destroy(this.gameObject);
    }
}
