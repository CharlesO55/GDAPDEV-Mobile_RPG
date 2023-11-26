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

            MultipleQuestsManager.Instance.CheckQuestEventOnObject(this.gameObject, EnumQuestAction.COLLECT);
            
            InteractableDetector.Instance.RemoveFromDetectedList(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void HighlightInteractable(bool bEnable)
    {
        Material mat = this.GetComponent<Renderer>().material;

        if (bEnable)
        {
            float emissiveIntensity = 2f;
            mat.EnableKeyword("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            mat.SetColor("_EmissionColor", Color.yellow * emissiveIntensity);
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black);
            mat.DisableKeyword("_EMISSION");
        }
    }
}
