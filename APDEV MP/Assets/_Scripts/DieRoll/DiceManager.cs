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
    [SerializeField] private readonly Vector3 _defaultDropPos = new Vector3(0, 5, 0);


    private bool _hasAlreadyRerolled = false;
    private RollArgs _rollArgs;
    private DieArgs _rollResult;
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

    
    public void DoRoll(bool isInstantaneousRoll = false, int nMin = 1, Vector3 rollPos = default, Vector3 throwDirection = default)
    {
        if (_dieObject.activeSelf)
        {
            //Debug.LogWarning("Tried to roll dice twice. To roll multiple dice simultaneously, modify DiceManager.DoRoll()");
            return;
        }

        //Overwrite if rolling from default
        rollPos = (rollPos == default) ?
            _defaultDropPos :
            rollPos;
        
        
        this._rollArgs = new RollArgs(nMin, isInstantaneousRoll, rollPos, throwDirection);


        //Create the dice
        this._dieObject.SetActive(true);
        
        this._dieObject.transform.position = _rollArgs.DropPosition;
        this.RandomizeDieRotation();


        this._dieObject.GetComponent<Rigidbody>().AddForce(throwDirection * 2, ForceMode.Impulse);

        //Set the camera
        CustomCameraSwitcher.Instance.SwitchCamera(EnumCameraID.DICE_CAM, this._dieObject);


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
        this.CheckResult();
    }

    private void RandomizeDieRotation()
    {
        Vector3 randRot;
        randRot.x = UnityEngine.Random.Range(-180, 180);
        randRot.y = UnityEngine.Random.Range(-180, 180);
        randRot.z = UnityEngine.Random.Range(-180, 180);

        _dieObject.transform.Rotate(randRot);
    }


    private void CheckResult()
    {
        int nResult = _dieObject.GetComponent<DieScript>().GetDieRollValue();
        Debug.Log("[Rolled] : " + nResult);

        //Store the result
        this._rollResult = new DieArgs(_rollArgs.MinRollValue, nResult, nResult >= _rollArgs.MinRollValue);

        //Disable the dice once done
        _dieObject.SetActive(false);




        //ALLOW REROLL WHEN 
        if (GameSettings.IS_REROLL_ENABLED && !_rollResult.RollPass && !_hasAlreadyRerolled)
        {
            this._hasAlreadyRerolled = true;

            UIManager.Instance.GetGameHUD().ToggleRerollUI(true);
        }
        else
        {
            this._hasAlreadyRerolled = false;
            //AdManager.Instance.OnAdCompleted -= this.TriggerReroll;
            
            this.BroadcastResult();
        }
    }

    public void TriggerReroll(object sender, EventArgs ignore)
    {
        this.DoRoll(_rollArgs.IsInstantaneousRoll, _rollArgs.MinRollValue, _rollArgs.DropPosition, _rollArgs.ThrowDirection);
    }

    public void BroadcastResult()
    {
        this.OnDiceResultObservsers?.Invoke(this, _rollResult);

        //Reset the camera
        CustomCameraSwitcher.Instance.SwitchCamera(EnumCameraID.PLAYER_CAM);
    }


}
