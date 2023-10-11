using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    [SerializeField] private string _playerName = "Unset";
    [SerializeField] private EnumUnitClass _class = EnumUnitClass.NONE;

    [SerializeField] private GameObject _playerModel;


    [Range(0, 20)][SerializeField] private int _strength;
    [Range(0, 20)] [SerializeField] private int _constitution;
    [Range(0, 20)] [SerializeField] private int _dexterity;
    [Range(0, 20)][SerializeField] private int _intelligence;
    [Range(0, 20)][SerializeField] private int _wisdom;
    [Range(0, 20)][SerializeField] private int _charisma;

    [Range(0, 20)][SerializeField] private int _maxHealth;
    [SerializeField] private int _currHealth;
}