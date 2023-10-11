using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public PartyManager Instance { get; private set; }

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
        SpawnCharacters();
    }

    void SpawnCharacters()
    {
        foreach (CharacterData entity in _partyMembers) {
            

        }
    }


    void Update()
    {
        
    }
}
