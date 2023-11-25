using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestGiver : MonoBehaviour , IInteractable
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
        Material mat = this.GetComponent<Renderer>().material;

        if (bEnable)
        {
            float emissiveIntensity = 2f;
            mat.EnableKeyword("_EMISSION");
            mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            mat.SetColor("_EmissionColor", Color.green * emissiveIntensity);
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.black);
            mat.DisableKeyword("_EMISSION");
        }
    }
}