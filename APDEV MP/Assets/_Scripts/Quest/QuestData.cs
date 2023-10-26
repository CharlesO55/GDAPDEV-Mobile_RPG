using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObjects/Quest")]
public class QuestData : ScriptableObject
{
    //THIS IS A SCRIPTABLE OBJECT
    //IT IS ONLY MEANT TO BE READ.
    //
    //DO NOT WRITE TO IT AS A METHOD OF TRACKING QUEST PROGRESS.
    //USE THE QUESTMANAGER INSTEAD
    
    [Header("Identifier")]
    [SerializeField] private EnumQuestID _questID;
    [SerializeField] private string _questName = "No name set";
    [SerializeField] private TextAsset _questStory;


    [Header("Prerequesites")]
    //level or other stuff needed first
    [SerializeField] private List<EnumQuestID> _prerequesiteQuests;


    [Header("Tasks/Goals to accomplish")]
    [SerializeField] private List<QuestStep> _questSteps;


    public EnumQuestID QuestID
    {
        get { return _questID; }
    }
    public string QuestName
    {
        get { return _questName; }
    }
    public TextAsset QuestStory
    {
        get { return _questStory; }
    }
    public List<EnumQuestID> PrerequesiteQuests
    {
        get { return _prerequesiteQuests; }
    }
    public List<QuestStep> QuestSteps
    {
        get { return _questSteps; }
    }
}