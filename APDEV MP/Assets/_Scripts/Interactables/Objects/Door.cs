using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] EnumObjectID _objectID = EnumObjectID.NONE;
    [SerializeField] int _targetScene;
    [SerializeField] int _spawnAreaIndex = 0;

    [Header("Door Locking")]
    [SerializeField] string _keyName;


    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK)
    {
        if (!string.IsNullOrEmpty(_keyName) && !InventoryManager.Instance.HasItem(_keyName))
        {
            UIManager.Instance.ChangeText("Door is LOCKED");
            Debug.Log($"{this.name} is LOCKED");
            return;
        }


        Debug.Log($"Used DOOR {this.name} to load Scene{_targetScene} at spawn zone {_spawnAreaIndex}");
        SceneLoaderManager.Instance.LoadScene(_targetScene, _spawnAreaIndex);
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
