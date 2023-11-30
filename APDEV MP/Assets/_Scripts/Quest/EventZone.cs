using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(DialogueStarter))]
public class EventZone : MonoBehaviour
{
    [SerializeField] private bool _isCollding;

    private void OnTriggerEnter(Collider other)
    {
        if(!_isCollding && this.TryGetComponent<IInteractable>(out IInteractable script))
        {
            _isCollding = true;
            script.OnInteractInterface(EnumQuestAction.MOVE);

            StartCoroutine(RestOnEndOfFrame());
        }
    }

    IEnumerator RestOnEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        _isCollding = false;
    }
}