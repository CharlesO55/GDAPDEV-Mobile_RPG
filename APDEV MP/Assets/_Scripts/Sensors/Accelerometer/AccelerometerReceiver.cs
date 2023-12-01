using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AccelerometerReceiver : MonoBehaviour
{
    [SerializeField] private AccelerometerProperty m_AcceleromterProperty;

    private void CheckAccelerometer()
    {
        if ((DialogueManager.Instance.IsRequestingRoll || CombatManager.Instance.IsRequestingRoll) && Mathf.Abs(Input.acceleration.x) >= this.m_AcceleromterProperty.MinChangeX)
            this.FireAccelerometerEvent();
    }

    private void FireAccelerometerEvent()
    {
        

        Debug.Log("Accelerometer Event Fired");
        DiceManager.Instance.DoRoll(false, DialogueManager.Instance.MinRollValue, default, Input.acceleration);
        DialogueManager.Instance.IsRequestingRoll = false;
        CombatManager.Instance.IsRequestingRoll = false;
    }

    private void FixedUpdate()
    {
        this.CheckAccelerometer();
    }
}
