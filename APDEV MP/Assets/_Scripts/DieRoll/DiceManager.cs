using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;

    [SerializeField] private GameObject _dieObject = null;
    [SerializeField] private float _diceRollWaitTime = 3f;
    [SerializeField] private Vector3 _diceRollStartPos;


    private int _nMinRollValue;
    public EventHandler<DieArgs> OnDiceResultObservsers;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Another DiceManager exists in this scene");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void DoRoll(bool isInstantaneousRoll = false, int nMin = 1)
    {
        if (_dieObject.activeSelf)
        {
            //Debug.LogWarning("Tried to roll dice twice. To roll multiple dice simultaneously, modify DiceManager.DoRoll()");
            return;
        }

        this._nMinRollValue = nMin;

        this._dieObject.SetActive(true);

        this._dieObject.transform.position = _diceRollStartPos;
        this.RandomizeDieRotation();

        if (!isInstantaneousRoll)
        {
            _dieObject.GetComponent<Rigidbody>().useGravity = true;
            _dieObject.GetComponent<Rigidbody>().velocity = Vector3.up;
            StartCoroutine(WaitForDiceValue(this._diceRollWaitTime));
        }
        else
        {
            _dieObject.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(WaitForDiceValue(0f));
        }
    }

    IEnumerator WaitForDiceValue(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        this.BroadcastValue();
    }

    private void RandomizeDieRotation()
    {
        Vector3 randRot;
        randRot.x = UnityEngine.Random.Range(-180, 180);
        randRot.y = UnityEngine.Random.Range(-180, 180);
        randRot.z = UnityEngine.Random.Range(-180, 180);

        _dieObject.transform.Rotate(randRot);
    }

    private void BroadcastValue()
    {
        int nResult = _dieObject.GetComponent<DieScript>().GetDieRollValue();
        Debug.Log("[Rolled] : " + nResult);

        this.OnDiceResultObservsers?.Invoke(this, new DieArgs(this._nMinRollValue, nResult, nResult >= _nMinRollValue));
        

        //Disable the dice once done
        _dieObject.SetActive(false);
    }
}
