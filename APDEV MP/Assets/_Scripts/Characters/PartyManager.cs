using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

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
        if(_spawnAreas.Count <= 0 || _spawnAreas[0] == null)
        {
            Debug.LogError("PartyManager has no SpawnAreas set");
        }

        if (isCreateDataFromField)
        {
            this.FirstTimeCreateCharacterData();
        }
        
        this._partyEntities = new List<GameObject>();
        SpawnCharacters();
        SwitchActiveCharacter();
    }

  
    void FirstTimeCreateCharacterData()
    {
        Debug.LogWarning("Saving party data from fields");

        SaveSystem.Save<CharacterData>(_newPartyMembers, SaveSystem.SAVE_FILE_ID.PARTY_DATA);
    }
    
    private void SpawnCharacters()
    {
        this._partyEntities.Clear();

        foreach (CharacterData _saveData in SaveSystem.LoadList<CharacterData>(SaveSystem.SAVE_FILE_ID.PARTY_DATA))
        {
            this._partyEntities.Add(CreateCharacter(_saveData));
        }
    }


    public bool SwitchActiveCharacter(int nIndex = -1, bool bRandom = false)
    {
        bool bSuccess = false;

        if (bRandom)
        {
            nIndex = UnityEngine.Random.Range(0, this._partyEntities.Count);
        }

        //FIND FIRST VALID CHARACTER. LOOP THROUGH EACH UNTIL FINDING A VALID ONE
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
                //TRY AGAIN BUT LOOKING FOR THE FIRST VALID
                bSuccess = SwitchActiveCharacter(-1);
            }
        }


        this.OnSwitchPlayerEvent?.Invoke(this, this._activePlayer);
        return bSuccess;
    }

    public void SwitchActiveCharacterByObject(GameObject member)
    {
        if (member.TryGetComponent<CharacterScript>(out CharacterScript charScript))
        {
            if (charScript.CanCharacterAct())
            {
                this._activePlayer = member;
                Debug.Log("[Switched] " + charScript.GetDetails());

                this.OnSwitchPlayerEvent?.Invoke(this, this._activePlayer);
            }
        }
    }

    public void SwitchActiveCharacterByName(string name)
    {
        GameObject m_CharacterToSwitch = this.FindCharacterByName(name);

        if (m_CharacterToSwitch != null)
            this.SwitchActiveCharacterByObject(m_CharacterToSwitch);
    }

    public GameObject FindCharacterByName(string name)
    {
        foreach (GameObject unit in this._partyEntities)
            if (unit.GetComponent<CharacterScript>().CharacterData.PlayerName == name)
                return unit;

        Debug.LogWarning($"No Character with name: {name}");
        return null;
    }

    public GameObject FindCharacterByClass(EnumUnitClass jobclass)
    {
        foreach (GameObject unit in this._partyEntities)
            if (unit.GetComponent<CharacterScript>().CharacterData.CharacterClass == jobclass)
                return unit;

        Debug.LogWarning($"No Character with class: {jobclass}");
        return null;
    }

    private GameObject CreateCharacter(CharacterData _saveData)
    {
        int spawnAreaIndex = SceneLoaderManager.Instance.SpawnAreaIndex;

        if (_spawnAreas.Count <= 0)
        {
            Debug.LogError("CreateCharacter failed : No spawn area set");
        }
        else if (spawnAreaIndex >= _spawnAreas.Count)
        {
            spawnAreaIndex = 0;
            Debug.LogWarning("SpawnAreaIndex is out of range");
        }
        //int spawnAreaIndex = UnityEngine.Random.Range(0, _spawnAreas.Count);

        GameObject characterObject = Instantiate(_saveData.CharacterModel, _spawnAreas[spawnAreaIndex].getRandomSpawnPos(), Quaternion.identity, this.transform);


        //ADD COMPONENTS TO OUR CHARACTERS or DIRECTLY USE A PREFAB
        characterObject.AddComponent<CharacterScript>().Init(_saveData);
        characterObject.AddComponent<NavMeshAgent>();
        

        if (characterObject.TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider))
        {
            capsuleCollider.enabled = true;
            capsuleCollider.isTrigger = true;
            capsuleCollider.radius = 0.2f;
        }

        characterObject.tag = "Ally";

        Debug.Log("[SPAWNED]" + characterObject.GetComponent<CharacterScript>().GetDetails());
        return characterObject;
    }

    public List<GameObject> PartyEntities { get { return this._partyEntities; } }
}