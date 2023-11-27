using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OreDeposit : MonoBehaviour , IInteractable
{
    [SerializeField] private EnumObjectID _objectID;
    [SerializeField] private int _strengthStatReq = 10;

    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK)
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
        if (bEnable)
        {
            Highlighter.HighlightObject(this.gameObject, Color.yellow);
        }
        else
        {
            Highlighter.HighlightObject(this.gameObject, Color.black);
        }
    }

    public EnumObjectID GetObjectID() { return _objectID; }
}
