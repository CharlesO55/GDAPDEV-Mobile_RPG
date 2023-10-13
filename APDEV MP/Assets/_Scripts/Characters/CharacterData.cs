using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    [SerializeField] private string _playerName = "Unset";
    [SerializeField] private EnumUnitClass _class = EnumUnitClass.NONE;

    [SerializeField] private GameObject _characterModel;


    [Range(0, 20)][SerializeField] private int _strength;
    [Range(0, 20)][SerializeField] private int _constitution;
    [Range(0, 20)][SerializeField] private int _dexterity;
    [Range(0, 20)][SerializeField] private int _intelligence;
    [Range(0, 20)][SerializeField] private int _wisdom;
    [Range(0, 20)][SerializeField] private int _charisma;

    [Range(0, 20)][SerializeField] private int _maxHealth;
    [SerializeField] private int _currHealth;


    //GETTERS SETTERS
    public string PlayerName { get { return _playerName; } }
    public EnumUnitClass CharacterClass { get { return _class; } }
    public GameObject CharacterModel { get { return _characterModel; } }




    [HideInInspector] public int Strength       { get { return this._strength; } }
    [HideInInspector] public int Constitution   { get { return this._constitution; } }
    [HideInInspector] public int Dexterity      { get { return this._dexterity; } }
    [HideInInspector] public int Intelligence   { get { return this._intelligence; } }
    [HideInInspector] public int Wisdom         { get { return this._wisdom; } }
    [HideInInspector] public int Charisma       { get { return this._charisma; } }

    [HideInInspector] public int MaxHealth      { get { return this._maxHealth; } }
    [HideInInspector] public int CurrHealth     { get { return this._currHealth; } set { this._currHealth = Mathf.Clamp(value, 0, this._maxHealth);  } }
}