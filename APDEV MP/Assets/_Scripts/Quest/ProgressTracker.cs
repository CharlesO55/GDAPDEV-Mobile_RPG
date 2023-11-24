using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTracker
{
    public int CurrentStep;
    public float GoalCollected;
    public float GoalNeeded { get; }

    public ProgressTracker(QuestData newQuest)
    {
        CurrentStep = -1;
        GoalCollected = 0;
        GoalNeeded = newQuest.QuestSteps[0].GoalAmount;
    }

    
    public bool IsGoalMet()
    {
        return GoalCollected >= GoalNeeded;
    }
}