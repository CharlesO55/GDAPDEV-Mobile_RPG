using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDestroyWhenTap : MonoBehaviour , ITappable
{
    public void OnTapInterface(TapEventArgs args)
    {
        Debug.Log("OH I GOT TAPPED");
        Destroy(this.gameObject);
    }
}
