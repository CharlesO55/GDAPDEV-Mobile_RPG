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

    private int m_STRModifier;
    private int m_CONModifier;
    private int m_DEXModifier;
    private int m_INTModifier;
    private int m_WISModifier;
    private int m_CHAModifier;

    private int m_Initiative = 0;

    //METHODS
    public void InitializeClass(EnumUnitClass unitClass)
    {
        switch(unitClass)
        {
            case EnumUnitClass.FIGHTER:
                this._strength      = 14;
                this._constitution  = 12;
                this._dexterity     = 10;
                this._intelligence  = 6;
                this._wisdom        = 8;
                this._charisma      = 10;
                break;

            case EnumUnitClass.PALADIN:
                this._strength      = 12;
                this._constitution  = 14;
                this._dexterity     = 4;
                this._intelligence  = 8;
                this._wisdom        = 12;
                this._charisma      = 10;
                break;

            case EnumUnitClass.ROGUE:
                this._strength      = 10;
                this._constitution  = 8;
                this._dexterity     = 14;
                this._intelligence  = 12;
                this._wisdom        = 10;
                this._charisma      = 6;
                break;

            case EnumUnitClass.MAGE:
                this._strength      = 8;
                this._constitution  = 4;
                this._dexterity     = 10;
                this._intelligence  = 14;
                this._wisdom        = 14;
                this._charisma      = 10;
                break;

            case EnumUnitClass.BARD:
                this._strength      = 6;
                this._constitution  = 8;
                this._dexterity     = 12;
                this._intelligence  = 10;
                this._wisdom        = 10;
                this._charisma      = 14;
                break;

            case EnumUnitClass.WANDERER:
                this._strength      = 10;
                this._constitution  = 10;
                this._dexterity     = 10;
                this._intelligence  = 10;
                this._wisdom        = 10;
                this._charisma      = 10;
                break;

            default:
                break;
        }

        this._class = unitClass;
        this.InitializeModifiers();
        this.InitializeHealthValues();
    }

    private void InitializeModifiers()
    {
        this.m_STRModifier = (this._strength - 8) / 2;
        this.m_CONModifier = (this._constitution - 8) / 2;
        this.m_DEXModifier = (this._dexterity - 8) / 2;
        this.m_INTModifier = (this._intelligence - 8) / 2;
        this.m_WISModifier = (this._wisdom - 8) / 2;
        this.m_CHAModifier = (this._charisma - 8) / 2;
    }

    private void InitializeHealthValues()
    {
        switch(this._class)
        {
            case EnumUnitClass.FIGHTER:
            case EnumUnitClass.PALADIN:
                this._maxHealth = 10 + this.m_CONModifier;
                break;

            case EnumUnitClass.ROGUE:
            case EnumUnitClass.BARD:
            case EnumUnitClass.WANDERER:
                this._maxHealth = 8 + this.m_CONModifier;
                break;

            case EnumUnitClass.MAGE:
                this._maxHealth = 6 + this.m_CONModifier;
                break;

            default:
                break;
        }

        this._currHealth = this._maxHealth;
    }

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

    [HideInInspector] public int STRMod         { get { return this.m_STRModifier; } }
    [HideInInspector] public int CONMod         { get { return this.m_CONModifier; } }
    [HideInInspector] public int DEXMod         { get { return this.m_DEXModifier; } }
    [HideInInspector] public int INTMod         { get { return this.m_INTModifier; } }
    [HideInInspector] public int WISMod         { get { return this.m_WISModifier; } }
    [HideInInspector] public int CHAMod         { get { return this.m_CHAModifier; } }

    [HideInInspector] public int MaxHealth      { get { return this._maxHealth; } }
    [HideInInspector] public int CurrHealth     { get { return this._currHealth; } set { this._currHealth = Mathf.Clamp(value, 0, this._maxHealth);  } }
    [HideInInspector] public int Initiative     { get { return this.m_Initiative; } set { this.m_Initiative = value;  } }
}