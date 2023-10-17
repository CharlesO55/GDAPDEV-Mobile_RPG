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
        PartyManager.Instance.SwitchActiveCharacter(0, true);

        Debug.Log("Debug ended");
    }
}