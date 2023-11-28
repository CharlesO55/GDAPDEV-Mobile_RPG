using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] EnumObjectID _objectID;
    [SerializeField] int _targetScene;
    [SerializeField] Door _door;

    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK)
    {
        Debug.Log($"Used DOOR {this.name}");
    }
    public virtual void HighlightInteractable(bool bEnable) 
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

    public EnumObjectID GetObjectID()
    {
        return _objectID;
    }
}
