using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDeposit : MonoBehaviour , IInteractable
{
    [SerializeField] private int _strengthStatReq = 10;

    public void OnInteractInterface()
    {
        DiceManager.Instance.OnDiceResultObservsers += CheckDiceRoll;
        DiceManager.Instance.DoRoll(false, _strengthStatReq);   
    }

    private void CheckDiceRoll(object sender, DieArgs args)
    {
        //Always unsubscribe first when roll finished
        DiceManager.Instance.OnDiceResultObservsers -= CheckDiceRoll;

        if (args.RollPass)
        {
            Debug.Log("Mined ore");


            InteractableDetector.Instance.RemoveFromDetectedList(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void HighlightInteractable(bool bEnable)
    {
        Material _mat = this.GetComponent<Renderer>().material;
        
        if(bEnable)
        {
            _mat.EnableKeyword("_EMISSION");
            _mat.SetColor("_EmissionColor", Color.yellow);
        }
        else
        {
            _mat.DisableKeyword("_EMISSION");
        }
    }
}
