using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueStarter : MonoBehaviour , IInteractable
{
    [SerializeField] private EnumObjectID _objectID;

    [SerializeField] private TextAsset _npcDialogue;

    [SerializeField] private List<QuestData> _quests;


    public void OnInteractInterface(EnumQuestAction questAction = EnumQuestAction.TALK)
    {
        //Prioritize continuining active quest dialogue

        if (MultipleQuestsManager.Instance.IsQuestTarget(out EnumQuestID questNeedingThis, this.gameObject, questAction))
        {
            MultipleQuestsManager.Instance.ProceedToNextStep(questNeedingThis);
        }


        else
        {
            //Try to start a new quest
            foreach (QuestData data in _quests)
            {
                if (MultipleQuestsManager.Instance.CanStartQuest(data))
                {
                    MultipleQuestsManager.Instance.TryStartQuest(data);
                    return;
                }
            }

            //Else do normal dialogue
            if (_npcDialogue != null)
            {

                DialogueManager.Instance.StartDialogue(_npcDialogue);
            }
            else
            {
                Debug.LogWarning($"{this.name} has no NPC Dialogue");
            }
        }
    }


    

    public void HighlightInteractable(bool bEnable)
    {
        if (bEnable)
        {
            Highlighter.HighlightObject(this.gameObject, Color.green);
        }
        else
        {
            Highlighter.HighlightObject(this.gameObject, Color.black);
        }
    }


    public EnumObjectID GetObjectID() { return _objectID; }
}