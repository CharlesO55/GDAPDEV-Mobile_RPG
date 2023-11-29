using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    EnumObjectID _objectID = EnumObjectID.DOOR;
    [SerializeField] int _targetScene;
    [SerializeField] int _spawnAreaIndex = 0;

    [SerializeField] bool _isLocked;


    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK)
    {
        if (this._isLocked)
        {
            Debug.Log($"{this.name} is LOCKED");
            return;
        }


        Debug.Log($"Used DOOR {this.name}");
        SceneLoaderManager.Instance.LoadScene(_targetScene);
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
