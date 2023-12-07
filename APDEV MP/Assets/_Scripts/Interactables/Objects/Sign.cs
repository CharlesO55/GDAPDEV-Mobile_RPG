using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Sign : MonoBehaviour, IInteractable
{
    [SerializeField] EnumObjectID _objectID = EnumObjectID.SIGN;

    [SerializeField] private TextAsset _dialogue;

    public EnumObjectID GetObjectID()
    {
        return _objectID;
    }

    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK)
    {
        if (_dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(_dialogue);
        }
        else
        {
            Debug.LogWarning($"{this.name} has no NPC Dialogue");
        }
    }

    public void HighlightInteractable(bool bEnable)
    {
        if (bEnable)
        {
            Highlighter.HighlightObject(this.gameObject, Color.gray);
        }
        else
        {
            Highlighter.HighlightObject(this.gameObject, Color.black);
        }
    }
}
