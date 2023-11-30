using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour
{
    private CharacterScript m_Character;

    private VisualElement root;
    private ProgressBar m_ProgressBar;

    private Image m_Interact;
    private Image m_Attack;

    private Label m_DiceRollLabel;

    private Label[] m_QuestLabels;
    private Label[] m_TaskLabels;

    [SerializeField]
    private AudioClip BattleBgm;
    private bool _inBattle = false;
    void OnEnable()
    {
        this.root = GetComponent<UIDocument>().rootVisualElement;
        this.m_ProgressBar = this.root.Q<ProgressBar>("Health");

        this.m_Interact = this.root.Q<Image>("InteractButton");
        this.m_Attack = this.root.Q<Image>("AttackButton");

        this.m_DiceRollLabel = this.root.Q<Label>("DiceRollLabel");

        this.ClickedImage(this.m_Interact, "Interact");
        this.ClickedImage(this.m_Attack, "Attack");

        
        //this.m_QuestLabel = this.root.Q<Label>("QuestLabel");
        //this.m_TaskLabel = this.root.Q<Label>("TaskLabel");


        m_QuestLabels = new Label[]{
            this.root.Q<Label>("QuestLabel"),
            this.root.Q<Label>("QuestLabel2"),
            this.root.Q<Label>("QuestLabel3")
        };

        m_TaskLabels = new Label[]{
            this.root.Q<Label>("TaskLabel"),
            this.root.Q<Label>("TaskLabel2"),
            this.root.Q<Label>("TaskLabel3")
        };
    }

    private void Start()
    {
        //this.m_Character = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>();

        this.m_ProgressBar.lowValue = 0;
        //this.m_ProgressBar.highValue = this.m_Character.CharacterData.MaxHealth;
    }

    private void Update()
    {
        this.m_ProgressBar.value = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CurrHealth;
        this.m_ProgressBar.highValue = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.MaxHealth;
        //this.m_ProgressBar.value = this.m_Character.CharacterData.CurrHealth;

        if (DialogueManager.Instance.IsRequestingRoll)
        {
            this.m_DiceRollLabel.style.display = DisplayStyle.Flex;
            this.m_DiceRollLabel.text = "Shake the Screen!";
        }

        else
            this.m_DiceRollLabel.style.display = DisplayStyle.None;
    }

    private void ClickedImage(Image img, string eventname)
    {
        if(!DebugginButton.isPaused)
        {
            img.AddManipulator(new Clickable(evt =>
            {
                switch (eventname)
                {
                    case "Interact":
                        Debug.Log("Interact Button Pressed");
                        if (InteractableDetector.Instance.TryGetClosestInteractableObject(out GameObject interactableObj))
                            {
                                if (interactableObj.TryGetComponent<IInteractable>(out IInteractable interactableInterface))
                                {
                                    Debug.Log("Interacted with " + interactableObj.name);
                                    interactableInterface.OnInteractInterface();
                                }
                            }
                            break;

                    case "Attack":
                        Debug.Log("Attack Button Pressed");
                        if(!CombatManager.Instance.IsInCombat)
                        {
                            CombatManager.Instance.BeginCombat();
                            MusicManager.instance.ChangeBGM(this.BattleBgm);
                        }

                        else
                        {
                            MusicManager.instance.RevertBGM();
                        }
                        //else
                        //{
                        //    this._inBattle = false;
                        //    MusicManager.instance.RevertBGM();
                        //}
                           
                        //DiceManager.Instance.DoRoll();
                        break;

                    default:
                        break;
                }
            }));
        }
   
    }


    public void UpdateQuestLabels(string[] strQuestNames = null, string[] strTaskInstructions = null)
    {
        int labelsUsed = 0;

        if (strQuestNames == null || strTaskInstructions == null)
        {
            m_QuestLabels[0].text = "No active quest";
            m_TaskLabels[0].text = "No task available";
            labelsUsed++;
        }

        else
        {
            while (labelsUsed < strQuestNames.Length && labelsUsed < m_QuestLabels.Length)
            {
                this.m_QuestLabels[labelsUsed].text = strQuestNames[labelsUsed];
                this.m_TaskLabels[labelsUsed].text = strTaskInstructions[labelsUsed];
                labelsUsed++;
            }
        }
        
        //THE REMAINING UNUSED LABELS
        while (labelsUsed < m_QuestLabels.Length)
        {
            this.m_QuestLabels[labelsUsed].text = "";
            this.m_TaskLabels[labelsUsed].text = "";
            labelsUsed++;
        }
    }
}
