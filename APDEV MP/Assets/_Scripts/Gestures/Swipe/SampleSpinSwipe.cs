using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSpinSwipe : MonoBehaviour , ISwipeable
{
    public void OnSwipeInterface(SwipeEventArgs args) {
        if (this.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.AddTorque(new Vector3(args.RawDirection.y, 0f, -args.RawDirection.x), ForceMode.Impulse);
        }
    
    }
}
