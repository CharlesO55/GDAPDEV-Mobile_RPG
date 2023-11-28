using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DialogueStarter))]
public class EventZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(this.TryGetComponent<IInteractable>(out IInteractable script))
        {
            script.OnInteractInterface(EnumQuestAction.MOVE);
        }
    }
}