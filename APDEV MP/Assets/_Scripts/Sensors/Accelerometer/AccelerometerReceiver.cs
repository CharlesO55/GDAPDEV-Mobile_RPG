using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerReceiver : MonoBehaviour
{
    [SerializeField] private AccelerometerProperty m_AcceleromterProperty;

    private void CheckAccelerometer()
    {
        if (DialogueManager.Instance.IsRequestingRoll && Mathf.Abs(Input.acceleration.x) >= this.m_AcceleromterProperty.MinChangeX)
            this.FireAccelerometerEvent();
    }

    private void FireAccelerometerEvent()
    {
        Debug.Log("Accelerometer Event Fired");
        DiceManager.Instance.DoRoll();
        DialogueManager.Instance.IsRequestingRoll = false;
    }

    private void FixedUpdate()
    {
        this.CheckAccelerometer();
    }
}
