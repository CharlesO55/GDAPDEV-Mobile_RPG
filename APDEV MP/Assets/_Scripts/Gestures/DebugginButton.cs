using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugginButton : MonoBehaviour , ITappable
{
    //THIS BUTTON IS PURELY FOR DEBUGGING. EXCLUDE FROM FINAL BUILD
    [SerializeField] UIDocument devOptions;
    // so that when dev menu is opened everything else doesnt update
    public static bool isPaused = false;

    public void OnTapInterface(TapEventArgs args)
    {
        Debug.Log("Debug start");

        //Stuff to test here...

        //[1] Dice rolling test
        //DiceManager.Instance.DoRoll(false);


        //[2] Player switching test
        //PartyManager.Instance.SwitchActiveCharacter(0, true);

        //[3] Debug the dice roll for Interactables
        //if (InteractableDetector.Instance.TryGetClosestInteractableObject(out GameObject interactableObj))
        //{
        //    if (interactableObj.TryGetComponent<IInteractable>(out IInteractable interactableInterface))
        //    {
        //        Debug.Log("Interacted with " + interactableObj.name);
        //        interactableInterface.OnInteractInterface();
        //    }
        //}
        // checks if dev options is not on screen the if not pauses the game and throws out the isPaused
        // bool that will be read by other update scripts and tells them not to update anything
        if(!devOptions.enabled)
        {
            isPaused = true;
            Time.timeScale = 0;
            devOptions.enabled = true;
        }
        




        Debug.Log("Debug ended");
    }
}