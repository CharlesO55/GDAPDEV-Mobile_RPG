using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugginButton : MonoBehaviour , ITappable
{
    //THIS BUTTON IS PURELY FOR DEBUGGING. EXCLUDE FROM FINAL BUILD

    public void OnTapInterface(TapEventArgs args)
    {
        Debug.Log("Debug start");

        //Stuff to test here...

        //[1] Dice rolling test
        //DiceManager.Instance.DoRoll(false);


        //[2] Player switching test
        //PartyManager.Instance.SwitchActiveCharacter(0, true);

        //[3] Debug the dice roll for Interactables
        if (InteractableDetector.Instance.TryGetClosestInteractableObject(out GameObject interactableObj))
        {
            if (interactableObj.TryGetComponent<IInteractable>(out IInteractable interactableInterface))
            {
                Debug.Log("Interacted with " + interactableObj.name);
                interactableInterface.OnInteractInterface();
            }
        }





        Debug.Log("Debug ended");
    }
}