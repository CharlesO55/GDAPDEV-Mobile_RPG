using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK);
    public virtual void HighlightInteractable(bool bEnable) { }

    public EnumObjectID GetObjectID();
}