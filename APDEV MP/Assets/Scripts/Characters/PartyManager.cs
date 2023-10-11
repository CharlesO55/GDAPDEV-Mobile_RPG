using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public PartyManager Instance { get; private set; }

    [SerializeField] private List<BoxTool> _spawnAreas;

    [SerializeField] private List<CharacterData> _partyMembers;

    private CharacterData activePlayer;



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
        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        SaveSystem.Save<CharacterData>(_partyMembers[1], SaveSystem.SAVE_FILE_ID.PARTY_DATA);
        //SaveSystem.Save<CharacterData>(_partyMembers[0], SaveSystem.SAVE_FILE_ID.PARTY_DATA);

        //SaveSystem.Save(_partyMembers);
        //SaveSystem.SaveCharacterData(_partyMembers);

        foreach (CharacterData _character in _partyMembers) {
            if(activePlayer == null)
            {
                activePlayer = _character;
            }

            if(_character.CharacterModel == null || _spawnAreas.Count <= 0)
            {
                Debug.LogError("Party Spawn Failed");
            }

            Instantiate(_character.CharacterModel, _spawnAreas[0].getRandomSpawnPos(), Quaternion.identity, this.transform);
        }

        //List<CharacterData> duahsdu = SaveSystem.LoadList<CharacterData>(SaveSystem.SAVE_FILE_ID.PARTY_DATA);
        //Debug.Log(duahsdu[0].CharacterModel.gameObject.name);

        CharacterData lom = SaveSystem.LoadSingle<CharacterData>(SaveSystem.SAVE_FILE_ID.PARTY_DATA);
        Debug.Log(lom.PlayerName);
    }
}