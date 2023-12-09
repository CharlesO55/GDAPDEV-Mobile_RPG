using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] EnumObjectID _objectID = EnumObjectID.NONE;
    
    [Header("Scene Loading")]
    [SerializeField] int _targetScene;
    [SerializeField] int _spawnAreaIndex = 0;
    [SerializeField] List<AssetLabelReference> _connectedRoomLabels;
    [SerializeField] bool _isSceneChange = true;


    [Header("Door Locking")]
    [SerializeField] string _keyName;


    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK)
    {
        if (!string.IsNullOrEmpty(_keyName) && !InventoryManager.Instance.HasItem(_keyName) && !GameSettings.IS_UNLOCK_ALL_DOORS)
        {
            SFXManager.Instance.PlaySFX(EnumSFX.SFX_DOOR_LOCKED);

            UIManager.Instance.ChangeText("Door is LOCKED");
            Debug.Log($"{this.name} is LOCKED");
            return;
        }


        Debug.Log($"Used DOOR {this.name} to load Scene{_targetScene} at spawn zone {_spawnAreaIndex}");
        if( _isSceneChange )
        {
            AssetSpawner.Instance.MarkNextSceneAssets(this._connectedRoomLabels);
            SceneLoaderManager.Instance.LoadScene(_targetScene, _spawnAreaIndex);
        }
        else
        {
            AssetSpawner.Instance.DespawnObjects();
            AssetSpawner.Instance.SpawnSceneObjects(this._connectedRoomLabels);
            PartyManager.Instance.MoveToSpawnLoc(_spawnAreaIndex);
        }

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
