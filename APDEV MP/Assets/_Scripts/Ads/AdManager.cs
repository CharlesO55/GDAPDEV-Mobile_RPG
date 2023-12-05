using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdManager : MonoBehaviour 
{
    public static AdManager Instance;

    [SerializeField] private InterstitialAd m_Ad;
    [SerializeField] private bool m_TestAd = false;


    //public EventHandler OnAdCompleted;

    // Start is called before the first frame update
    public void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(this.gameObject);
    }

    private void Update()
    {
        if (this.m_TestAd)
        {
            Debug.Log("Start ad");
            this.ShowAd();
            this.m_TestAd = false;
        }
    }

    public void ShowAd()
    {
        this.m_Ad.ShowAd();
    }

    public void MarkAdAsCompleted()
    {
        DiceManager.Instance.TriggerReroll(this, null);
        //this.OnAdCompleted?.Invoke(this, null);
    }
}
