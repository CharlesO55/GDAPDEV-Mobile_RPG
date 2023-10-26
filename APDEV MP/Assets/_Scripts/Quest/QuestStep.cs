using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestStep
{
    //Attributes
    [SerializeField] private string _instructions = "No instruction set";
    [SerializeField] private EnumQuestAction _action = EnumQuestAction.NONE;

    [SerializeField] private float _goalAmount;
    [SerializeField] private GameObject _goalObject;

    //[SerializeField] private TextAsset _dialogueAtStart;
    //ACCESS
    public string Instructions
    {
        get { return _instructions; }
    }
    public EnumQuestAction Action { 
        get { return _action; } 
    }

    public float GoalAmount
    {
        get { return _goalAmount; }
    }
    public GameObject GoalObject
    {
        get { return _goalObject; }
    }
    /*public TextAsset DialogueAtStart
    {
        get { return _dialogueAtStart; }
    }*/
}