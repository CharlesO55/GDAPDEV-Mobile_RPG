using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AccelerometerProperty
{
    [SerializeField] private float m_SpeedX = 30.0f;
    [SerializeField] private float m_MinChangeX = 1.0f;

    public float SpeedX { get { return this.m_SpeedX; } set { this.m_SpeedX = value; } }
    public float MinChangeX { get { return this.m_MinChangeX; } set { this.m_MinChangeX = value; } }
}
