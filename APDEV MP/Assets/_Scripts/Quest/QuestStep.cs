using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestStep
{
    //Attributes
    [SerializeField] private int _questStepIndex;
    [SerializeField] private string _instructions = "No instruction set";
    [SerializeField] private EnumQuestAction _action = EnumQuestAction.NONE;

    private float _currAmount = 0;
    [SerializeField] private float _goalAmount;
    [SerializeField] private GameObject _goalObject;

    //ACCESS
    public int QuestStepIndex
    {
        get { return _questStepIndex; }
    }
    public string Instructions
    {
        get { return _instructions; }
    }
    public EnumQuestAction Action 
    { 
        get { return _action; } 
    }

    public float CurrentAmount
    {
        get { return this._currAmount; }
    }
    public float GoalAmount
    {
        get { return _goalAmount; }
    }
    public GameObject GoalObject
    {
        get { return _goalObject; }
    }
    

    public QuestStep(QuestStep toCopy)
    {
        this._questStepIndex = toCopy._questStepIndex;
        this._instructions = toCopy._instructions;
        
        this._currAmount = toCopy._currAmount;
        this._goalAmount = toCopy._goalAmount;

        this._goalObject = toCopy._goalObject;
        this._action = toCopy._action;
    }
    public void ResetCurrAmount()
    {
        this._currAmount = 0;
    }
    public void IncreaseCurrAmount()
    {
        this._currAmount++;
    }
    public bool IsGoalAmountReached()
    {
        return _currAmount >= _goalAmount;
    }

    public void Summarize()
    {
        Debug.Log($"[#{_questStepIndex}] {_currAmount}/{_goalAmount} {_instructions}");
    }
}