/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleQuestsManager : MonoBehaviour
{
    public static MultipleQuestsManager Instance;

    //Quest tracking
    [SerializeField] private List<EnumQuestID> _completedQuests = new();


    //This is purely for visibility in inspector.
    //[SerializeField] private QuestData _questReference;
    [SerializeField] private List<QuestData> _activeQuests = new();

    //Progress tracking
    //[SerializeField] private int _nCurrentStepIndex = -1;
    //[SerializeField] private float _fCurrentGoalAmount;
    private Dictionary<EnumQuestID, ProgressTracker> _progressTrackers = new();



    //Tracking Player Morality Values
    [SerializeField] private int m_PlayerMorality = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        this.UpdateUIQuestInfo();
    }




    ******************
     * STARTING QUESTS *
     *****************
    public bool IsQuestActive()
    {
        return this._activeQuests.Count != 0;
    }

    public bool CanStartQuest(QuestData newQuest)
    {
        if (_completedQuests.Contains(newQuest.QuestID) || _activeQuests.Contains(newQuest))
        {
            Debug.LogWarning("Quest Rejected. Already taken");
            return false;
        }

        //Verify the quest data
        if (newQuest.QuestSteps.Count == 0)
        {
            Debug.LogError("Quest Rejected. Quest contains no steps");
            return false;
        }

        return true;
    }







    /*********************
    * PROGRESSING QUESTS *
    *********************
    private EnumQuestID MatchQuestIDToName(string strQuestName)
    {
        if (this._questReference.QuestName == strQuestName)
            return _questReference.QuestID;

        else
        {
            Debug.LogError($"FAILED to find mathcing quest ID to {strQuestName}");
            return EnumQuestID.NULL;
        }
    }

    public void CheckQuestEventOnObject(GameObject sender, EnumQuestAction actionOccured)
    {
        if (!IsQuestActive())
        {
            return;
        }

        bool isTargetObj = sender.GetType().IsInstanceOfType(GetCurrentObjective().GoalObject);
        bool isDesiredAction = actionOccured == GetCurrentObjective().Action;


        if (isTargetObj && isDesiredAction)
        {
            this._fCurrentGoalAmount++;
            this.UpdateUIQuestInfo();
            Debug.Log("Increased progress");

            if (this._fCurrentGoalAmount >= GetCurrentObjective().GoalAmount)
            {
                this.ProceedToNextStep();
            }
        }
    }



    //THIS VER ALLLOWS FOR EXTERNAL WRITING OF THE CURRENT QUEST STEP.
    //IDEALLY TO BE USED BY DIALOGUE MANAGER SINCE DIFFERENT OPTIONS CAN LEAD TO DIFFERENT STEPS.
    public void SetNextStep(int nNextStep)
    {
        Debug.Log("Next step" + nNextStep);

        this._nCurrentStepIndex = nNextStep;

        //QUEST WAS RESET BY DIALOGUE OPTION
        if (nNextStep < 0)
        {
            ResetCurrQuest();
        }

        //QUEST HAS REACHED END
        else if (GetCurrentObjective().Action == EnumQuestAction.END)
        {
            this.EndQuest();
        }

        UpdateUIQuestInfo();
    }

    private void UpdateUIQuestInfo()
    {
        if (!this.IsQuestActive())
        {
            UIManager.Instance.GetGameHUD().UpdateQuestLabels();
        }
        else
        {
            List<string> questNames = new();
            List<string> taskInstructions = new();

            foreach (QuestData quest in _activeQuests)
            {
                questNames.Add(quest.QuestName);

                int currStep = _progressTrackers[quest.QuestID].CurrentStep;
                questNames.Add(quest.QuestSteps[currStep].Instructions);

                if (questNames.Count > 3)
                {
                    break;
                }
            }

            //string strInstructions = $"({this._fCurrentGoalAmount}/{this.GetCurrentObjective().GoalAmount}) {this.GetCurrentObjective().Instructions}";
            UIManager.Instance.GetGameHUD().UpdateQuestLabels(questNames.ToArray(), taskInstructions.ToArray());
        }
    }

    private void ResetCurrQuest()
    {
        this._nCurrentStepIndex = -1;
        this._fCurrentGoalAmount = 0;
        this._questReference = null;
    }

    public bool TryStartQuest(QuestData newQuest)
    {
        if (!CanStartQuest(newQuest))
        {
            return false;
        }

        Debug.Log("Started new quest: " + newQuest.QuestName);
        this._nCurrentStepIndex = 0;
        this._questReference = newQuest;
        this.ProceedToNextStep();

        return true;
    }


    //CALLED WHEN THE GOAL AMOUNT IS MET
    //OR COMPLETED EVENT
    //STEPS OF DIALOGUE RELATED CHOICES USE SetNextStep() in
    public void ProceedToNextStep()
    {
        if (!this.IsQuestActive())
        {
            return;
        }

        //this._nCurrentStepIndex++;
        this._fCurrentGoalAmount = 0;

        //Debug.Log("New step: " + this._questReference.QuestSteps[_nCurrentStepIndex].Instructions);

        DialogueManager.Instance.StartDialogue(this._questReference.QuestStory, _nCurrentStepIndex);
    }

    private void EndQuest()
    {
        Debug.Log("Quest completed: " + this._questReference.QuestName);
        this.m_PlayerMorality += 5;
        this._completedQuests.Add(this._questReference.QuestID);

        //Reset the progress tracker
        this.ResetCurrQuest();
    }




    public QuestStep GetCurrentObjective()
    {
        return this._questReference.QuestSteps[_nCurrentStepIndex - 1];
    }

    public bool IsQuestTarget(GameObject targetObj, EnumQuestAction actionToCheck = EnumQuestAction.NONE)
    {
        if (!IsQuestActive())
        {
            return false;
        }


        if (GetCurrentObjective().GoalObject.GetType().IsInstanceOfType(targetObj))
        {
            if (actionToCheck != EnumQuestAction.NONE)
            {
                if (actionToCheck == GetCurrentObjective().Action)
                {
                    //Same object and action
                    return true;
                }
                //Same object but different action
                return false;
            }
            //Same object
            return true;
        }
        //Different object
        return false;
    }

    public int PlayerMorality { get { return this.m_PlayerMorality; } }
}
*/