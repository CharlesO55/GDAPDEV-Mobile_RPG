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
        //Prioritize 
        if (QuestManager.Instance.IsQuestTarget(this.gameObject, EnumQuestAction.TALK))
        {
            QuestManager.Instance.ProceedToNextStep();
        }


        else
        {
            //Try to start a new quest
            foreach (QuestData data in _quests)
            {
                if (QuestManager.Instance.CanStartQuest(data))
                {
                    QuestManager.Instance.TryStartQuest(data);
                    return;
                }
            }

            //Else do normal dialogue
            DialogueManager.Instance.StartDialogue(_npcDialogue);
        }
    }


    

    public void HighlightInteractable(bool bEnable)
    {
        Material _mat = this.GetComponent<Renderer>().material;

        if (bEnable)
        {
            _mat.EnableKeyword("_EMISSION");
            _mat.SetColor("_EmissionColor", Color.yellow);
        }
        else
        {
            _mat.DisableKeyword("_EMISSION");
        }
    }
}