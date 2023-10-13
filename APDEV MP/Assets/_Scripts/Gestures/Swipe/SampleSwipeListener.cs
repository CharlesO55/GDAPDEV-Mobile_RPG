using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSwipeListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GestureManager.Instance.OnSwipeDelegate += SayName;   
    }

    // Update is called once per frame
    void OnDisable()
    {
        GestureManager.Instance.OnSwipeDelegate -= SayName;
    }

    void SayName(object sender, SwipeEventArgs e)
    {
        Debug.Log("I heard swipe towards " + e.Direction);
    }
}
