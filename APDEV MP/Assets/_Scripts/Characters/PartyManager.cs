using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    [Header("Spawning")]
    [SerializeField] private List<BoxTool> _spawnAreas;

    private PlayerController m_PlayerController;
    [SerializeField] CameraTargetter m_CameraTargetter;

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

    [Header("Alternate Solution")]
    public CharacterData MainPlayerData;


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
        this.m_PlayerController = this.GetComponent<PlayerController>();

        if (this.m_PlayerController == null)
            Debug.Log("PlayerController is Null");

        this.CreateSingleCharacter();
        
        //Subvert for now
        /*if (isCreateDataFromField)
        {
            this.FirstTimeCreateCharacterData();
        }
        
        this._partyEntities = new List<GameObject>();
        SpawnCharacters();
        SwitchActiveCharacter(-1);*/
    }

    //Subvert until save system works
/*
    void FirstTimeCreateCharacterData()
    {
        Debug.Log("Saving party data from fields");

        SaveSystem.Save<CharacterData>(_newPartyMembers, SaveSystem.SAVE_FILE_ID.PARTY_DATA);
    }

    private void SpawnCharacters()
    {
        this._partyEntities.Clear();


        //Subvert save system for now
        //foreach (CharacterData _saveData in SaveSystem.LoadList<CharacterData>(SaveSystem.SAVE_FILE_ID.PARTY_DATA))
        //{
        //    this._partyEntities.Add(CreateCharacter(_saveData));
        //}

        foreach (CharacterData newCharData in  _newPartyMembers)
        {
            this._partyEntities.Add(CreateCharacter(newCharData));
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

        this.m_PlayerController.UpdatePlayerRef(this, this._activePlayer);
        this.m_CameraTargetter.SetTarget(this, this._activePlayer);
        this.OnSwitchPlayerEvent?.Invoke(this, this._activePlayer);
        return bSuccess;
    }*/

    /*private GameObject CreateCharacter(CharacterData _saveData)
    {
        if(_spawnAreas.Count <= 0)
        {
            Debug.LogError("CreateCharacter failed : No spawn area set");
        }


        int rngSpawn = UnityEngine.Random.Range(0, _spawnAreas.Count);

        GameObject characterObject = Instantiate(_saveData.CharacterModel, _spawnAreas[rngSpawn].getRandomSpawnPos(), Quaternion.identity, this.transform);


        //ADD COMPONENTS TO OUR CHARACTERS or DIRECTLY USE A PREFAB
        characterObject.AddComponent<CharacterScript>().Init(_saveData);
        characterObject.AddComponent<NavMeshAgent>();
        
        Debug.Log("[SPAWNED]" + characterObject.GetComponent<CharacterScript>().GetDetails());


        //_rb is automatically for imports... Set to kinematic instead to disable interference with navmesh agent
        Rigidbody _rb = characterObject.GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        //_rb.interpolation = RigidbodyInterpolation.Interpolate;
        

        return characterObject;
    }*/

    private void CreateSingleCharacter()
    {
        int rngSpawn = UnityEngine.Random.Range(0, _spawnAreas.Count);

        GameObject playerInstance = Instantiate(this.MainPlayerData.CharacterModel, _spawnAreas[rngSpawn].getRandomSpawnPos(), Quaternion.identity, this.transform);
        playerInstance.AddComponent<CharacterScript>().Init(this.MainPlayerData);
        playerInstance.AddComponent<NavMeshAgent>();

        /*Rigidbody _rb = playerInstance.GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
*/
        if (playerInstance.TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider))
        {
            capsuleCollider.enabled = false;
        }

        this._activePlayer = playerInstance;

        this.m_PlayerController.UpdatePlayerRef(this, this._activePlayer);
        this.m_CameraTargetter.SetTarget(this, this._activePlayer);
    }
}