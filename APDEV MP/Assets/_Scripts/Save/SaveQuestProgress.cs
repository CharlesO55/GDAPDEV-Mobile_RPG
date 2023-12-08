using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestProgressSave
{
    public int PlayerMorality;

    public List<QuestData> ActiveQuests;
    public List<EnumQuestID> CompletedQuestIDs;


    public List<EnumQuestID> StepTrackerKeys;
    public List<QuestStep> StepTrackerValues;
    public QuestProgressSave(int morality, List<QuestData> activeQ, List<EnumQuestID> completedQ, List<EnumQuestID> stepKeys, List<QuestStep> stepValues) 
    { 
        this.PlayerMorality = morality;
        this.ActiveQuests = activeQ;
        this.CompletedQuestIDs = completedQ;

        this.StepTrackerKeys = stepKeys;
        this.StepTrackerValues = stepValues;
    }
}
