using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    [Header("Spawning")]
    [SerializeField] private List<BoxTool> _spawnAreas;
    
    private List<GameObject> _partyEntities;
    private GameObject _activePlayer;
    public GameObject ActivePlayer
    {
        get { return _activePlayer; }
    }


    [Header("Creating new party data")]
    [Tooltip("Enable to create party data from fields, otherwise from save data")]
    [SerializeField] private bool isCreateDataFromField;
    [SerializeField] private List<CharacterData> _newPartyMembers;

    public EventHandler<GameObject> OnSwitchPlayerEvent;
    

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        if (isCreateDataFromField)
        {
            this.FirstTimeCreateCharacterData();
        }
        
        this._partyEntities = new List<GameObject>();
        SpawnCharacters();
        SwitchActiveCharacter(-1);
    }



    void FirstTimeCreateCharacterData()
    {
        Debug.Log("Saving party data from fields");

        SaveSystem.Save<CharacterData>(_newPartyMembers, SaveSystem.SAVE_FILE_ID.PARTY_DATA);
    }

    private void SpawnCharacters()
    {
        this._partyEntities.Clear();

        foreach (CharacterData _saveData in SaveSystem.LoadList<CharacterData>(SaveSystem.SAVE_FILE_ID.PARTY_DATA))
        {
            this._partyEntities.Add(CreateCharacter(_saveData));
        }

        this._activePlayer = this._partyEntities[0];
    }


    public bool SwitchActiveCharacter(int nIndex = -1, bool bRandom = false)
    {
        bool bSuccess = false;

        if (bRandom)
        {
            nIndex = UnityEngine.Random.Range(0, this._partyEntities.Count);
        }

        //LOOP THROUGH EACH UNTIL FINDING A VALID ONE
        if (nIndex == -1)
        {
            foreach(GameObject _partyMember in this._partyEntities)
            {
                if (_partyMember.TryGetComponent<CharacterScript>(out CharacterScript charScript)) 
                {
                    if (charScript.CanCharacterAct())
                    {
                        this._activePlayer = _partyMember;
                        bSuccess = true;
                        Debug.Log("[Switched] " + charScript.GetDetails());
                        break;
                    }
                }
            }
        }

        //FOR SWITCHING TO A SPECIFIED INDEX
        else
        {
            nIndex = Mathf.Clamp(nIndex, 0, this._partyEntities.Count);
            if (this._partyEntities[nIndex].TryGetComponent<CharacterScript>(out CharacterScript charScript)) {
                if (charScript.CanCharacterAct())
                {
                    this._activePlayer = this._partyEntities[nIndex];
                    Debug.Log("[Switched] " + charScript.GetDetails());
                    bSuccess = true;
                }
            }
            else
            {
                bSuccess = SwitchActiveCharacter(-1);
            }
        }

        this.OnSwitchPlayerEvent?.Invoke(this, this._activePlayer);
        return bSuccess;
    }

    private GameObject CreateCharacter(CharacterData _saveData)
    {
        if(_spawnAreas.Count <= 0)
        {
            Debug.LogError("CreateCharacter failed : No spawn area set");
        }


        int rngSpawn = UnityEngine.Random.Range(0, _spawnAreas.Count);

        GameObject characterObject = Instantiate(_saveData.CharacterModel, _spawnAreas[rngSpawn].getRandomSpawnPos(), Quaternion.identity, this.transform);

        characterObject.AddComponent<CharacterScript>().Init(_saveData);

        Debug.Log("[SPAWNED]" + characterObject.GetComponent<CharacterScript>().GetDetails());

        return characterObject;
    }
}