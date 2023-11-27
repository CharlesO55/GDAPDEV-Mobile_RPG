using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueStarter : MonoBehaviour , IInteractable
{
    [SerializeField] private TextAsset _npcDialogue;

    [SerializeField] private List<QuestData> _quests;


    public void OnInteractInterface()
    {
        //Prioritize continuining active quest dialogue
        
        if(MultipleQuestsManager.Instance.IsQuestTarget(out EnumQuestID questNeedingThis, this.gameObject, EnumQuestAction.TALK))
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
            DialogueManager.Instance.StartDialogue(_npcDialogue);
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
}