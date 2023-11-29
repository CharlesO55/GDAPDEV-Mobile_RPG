using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultipleQuestsManager : MonoBehaviour
{
    public static MultipleQuestsManager Instance;

    //Quest tracking
    [SerializeField] private List<EnumQuestID> _completedQuestIDs = new();


    //This is purely for visibility in inspector.
    //[SerializeField] private QuestData _questReference;
    [SerializeField] private List<QuestData> _activeQuests = new();

    //Progress tracking
    private Dictionary<EnumQuestID, QuestStep> _stepTrackers = new();


    //Tracking Player Morality Values
    [SerializeField] private int m_PlayerMorality = 0;
    public int PlayerMorality { get { return this.m_PlayerMorality; } }


    /****************************
     *      LIFECYCLE FUNCS     *
     ***************************/
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += CallUIUpdate;
        this.UpdateUIQuestInfo();
    }

    private void CallUIUpdate(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            this.UpdateUIQuestInfo();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CallUIUpdate;
        Destroy(this.gameObject);
    }


    /********************************
     *      STARTING QUESTS         *
     ********************************/
    public bool IsQuestActive()
    {
        return this._activeQuests.Count != 0;
    }

    public bool CanStartQuest(QuestData newQuest)
    {
        //Start only new quests
        if (_completedQuestIDs.Contains(newQuest.QuestID) || _activeQuests.Contains(newQuest))
        {
            Debug.LogWarning($"Quest Rejected. {newQuest.QuestName} was already taken");
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

    public bool TryStartQuest(QuestData newQuest)
    {
        if (!CanStartQuest(newQuest))
        {
            return false;
        }

        Debug.Log("Started new quest: " + newQuest.QuestName);

        //REGISTER THE NEW QUEST
        this._activeQuests.Add(newQuest);

        //START AT THE FIRST STEP
        //COPY IT AS INSTANCE INSTEAD OF DIRECTLY EDITING IT
        this._stepTrackers[newQuest.QuestID] = new QuestStep(newQuest.QuestSteps[0]);
        
        //TRIGGER THE FIRST DIALOGUE IN THE QUESTLINE
        DialogueManager.Instance.StartDialogue(newQuest.QuestStory);

        return true;
    }




    /**********************************
    *      Progressing Quests        *
    **********************************/
    /*********************************************************************
     * IMPORTANT FUNCS:                                                  *
     * SetNextStep() is the primary method changing steps                *
     *      - Will primarily be triggered by DialogueChoices/Completion  *
     * ProceedToNextStep is what tells the Dialogue to start             *
     *      - Will be trigggered by GoalAmount reached checks            *
     * ******************************************************************/
    public void ProceedToNextStep(QuestData questReference)
    {
        if (!this.IsQuestActive())
        {
            return;
        }
        else if(questReference == null)
        {
            Debug.LogError("Error ProceedToNextStep received null QuestData");
        }

        //Verify
        Debug.Log($"{questReference.QuestName} Proceeding to next step");
        this._stepTrackers[questReference.QuestID].Summarize();
        
        
        this._stepTrackers[questReference.QuestID].ResetCurrAmount();


        //TRIGGER THE NEXT DIALOGUE
        DialogueManager.Instance.StartDialogue(questReference.QuestStory, this._stepTrackers[questReference.QuestID].QuestStepIndex+1);
    }

    public void ProceedToNextStep(EnumQuestID ID)
    {
        this.ProceedToNextStep(this._activeQuests.First(a => a.QuestID == ID));
    }


    /*********************************************************************
     * IMPORTANT FUNCS:                                                  *
     * SetNextStep() is the primary method changing steps                *
     *      - Will primarily be triggered by DialogueChoices/Completion  *
     * ProceedToNextStep is what tells the Dialogue to start             *
     *      - Will be trigggered by GoalAmount reached checks            *
     * ******************************************************************/

    public void SetNextStep(int nNextStep, string strQuestName)
    {
        nNextStep -= 1;
        Debug.Log("Next step" + nNextStep);

        //SET THE NEXT STEP
        QuestData reference = FindQuestFromName(strQuestName);


        //QUEST WAS RESET BY DIALOGUE OPTION
        if (nNextStep < 0)
        {
            ResetQuest(reference.QuestID);
            UpdateUIQuestInfo();
            return;
        }

        //SET THE NEXT STEP
        this._stepTrackers[reference.QuestID] = new QuestStep(reference.QuestSteps[nNextStep]);
        this._stepTrackers[reference.QuestID].Summarize();
        
        //QUEST HAS REACHED END
        if (this._stepTrackers[reference.QuestID].Action == EnumQuestAction.END)
        {
            this.EndQuest(reference);
        }
        

        UpdateUIQuestInfo();
    }



    private QuestData FindQuestFromName(string strQuestName)
    {
        QuestData reference = this._activeQuests.First(a => a.QuestName == strQuestName);
        if (reference == null)
        {
            Debug.LogError($"FAILED to find mathcing quest ID to {strQuestName}");
        }

        return reference;
    }

    public void CheckQuestEventOnObject(GameObject sender, EnumQuestAction actionOccured)
    {
        if (!IsQuestActive())
        {
            return;
        }

        
        if (IsQuestTarget(out EnumQuestID questIDFound, sender.gameObject, actionOccured))
        {
            this._stepTrackers[questIDFound].IncreaseCurrAmount();


            this.UpdateUIQuestInfo();
            Debug.Log("Increased progress");

            if (this._stepTrackers[questIDFound].IsGoalAmountReached())
            {
                this.ProceedToNextStep(this._activeQuests.First(a => a.QuestID == questIDFound));
            }
        }
    }

    public bool IsQuestTarget(out EnumQuestID belongsToQuest, GameObject targetObj, EnumQuestAction actionToCheck = EnumQuestAction.NONE)
    {
        belongsToQuest = EnumQuestID.NULL;

        if (!IsQuestActive())
        {
            return false;
        }



        foreach (QuestData activeQuest in this._activeQuests)
        {
            QuestStep currObjective = GetCurrentObjective(activeQuest);

            
            if(currObjective.Action == actionToCheck)
            {
                //Debug.Log($"{currObjective}")

                if(currObjective.GoalObject.TryGetComponent<IInteractable>(out IInteractable currIInteractable) && targetObj.TryGetComponent(out IInteractable targetIInteractable))
                {
                    if(currIInteractable.GetObjectID() == targetIInteractable.GetObjectID())
                    {
                        belongsToQuest = activeQuest.QuestID;
                        return true;
                    }
                }

            }
            else if (currObjective.Action == EnumQuestAction.NONE)
            {
                Debug.LogError("QuestAction can't be NONE");
            }
            /*if (currObjective.GoalObject.GetType().IsInstanceOfType(targetObj))
            {
                if (actionToCheck != EnumQuestAction.NONE)
                {
                    if (actionToCheck == currObjective.Action)
                    {
                        //Same object and action
                        belongsToQuest = activeQuest.QuestID;
                        return true;
                    }
                    //Same object but different action
                    return false;
                }
                //Same object
                belongsToQuest = activeQuest.QuestID;
                return true;
            }*/
        }

        //Different object
        return false;

    }

    public QuestStep GetCurrentObjective(QuestData questRef)
    {
        return this._stepTrackers[questRef.QuestID];
    }


    /**********************************
     *        Ending Quests           *
     *********************************/
    private void ResetQuest(EnumQuestID questID)
    {
        Debug.Log("Resetting quest...");
        this._stepTrackers.Remove(questID);

        
        //UNREGISTER THE QUEST
        QuestData toRemove = this._activeQuests.First(a => a.QuestID == questID);
        if(toRemove == null)
        {
            Debug.LogError($"{questID} is not registered in _activeQuests" );
        }
        this._activeQuests.Remove(toRemove);
    }
    private void EndQuest(QuestData questCompleted)
    {
        Debug.Log($"Completed {questCompleted.QuestName}");
     
        //REGISTER TO COMPLETED QUESTS
        this._completedQuestIDs.Add(questCompleted.QuestID);
        //UNREGISTER FROM ACTIVE
        this._activeQuests.Remove(questCompleted);

        this.m_PlayerMorality += 5;
    }


    /**********************************
     *          UI Update             *
     *********************************/
    IEnumerator UpdateUIInMomment()
    {
        yield return new WaitForSeconds(2);
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


                QuestStep currObjective = GetCurrentObjective(quest);
                taskInstructions.Add($"({currObjective.CurrentAmount}/{currObjective.GoalAmount}) {currObjective.Instructions}");
            }
            UIManager.Instance.GetGameHUD().UpdateQuestLabels(questNames.ToArray(), taskInstructions.ToArray());
        }
    }
}