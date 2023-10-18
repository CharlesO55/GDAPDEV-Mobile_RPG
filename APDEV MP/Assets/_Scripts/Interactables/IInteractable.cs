using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteractInterface();
    public virtual void HighlightInteractable(bool bEnable) { }
}