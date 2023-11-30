using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour
{
    private CharacterScript m_Character;

    private VisualElement root;

    private Image m_Interact;
    private Image m_Attack;

    private Image m_Player1Portrait;
    private Image m_Player2Portrait;
    private Image m_Player3Portrait;
    private Image m_Player4Portrait;

    private VisualElement[] m_PlayerFrames;
    private Label[] m_PlayerHealth;

    private Label m_DiceRollLabel;

    private Label[] m_QuestLabels;
    private Label[] m_TaskLabels;

    [SerializeField]
    private AudioClip BattleBgm;

    void OnEnable()
    {
        this.root = GetComponent<UIDocument>().rootVisualElement;

        this.m_Interact = this.root.Q<Image>("InteractButton");
        this.m_Attack = this.root.Q<Image>("AttackButton");

        this.m_Player1Portrait = this.root.Q<Image>("Player1Image");
        this.m_Player2Portrait = this.root.Q<Image>("Player2Image");
        this.m_Player3Portrait = this.root.Q<Image>("Player3Image");
        this.m_Player4Portrait = this.root.Q<Image>("Player4Image");

        this.m_DiceRollLabel = this.root.Q<Label>("DiceRollLabel");

        this.ClickedImage(this.m_Interact, "Interact");
        this.ClickedImage(this.m_Attack, "Attack");

        this.ClickedImage(this.m_Player1Portrait, "SwitchToP1");
        this.ClickedImage(this.m_Player2Portrait, "SwitchToP2");
        this.ClickedImage(this.m_Player3Portrait, "SwitchToP3");
        this.ClickedImage(this.m_Player4Portrait, "SwitchToP4");
        
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

        this.m_PlayerFrames = new VisualElement[]
        {
            this.root.Q<VisualElement>("Player1Frame"),
            this.root.Q<VisualElement>("Player2Frame"),
            this.root.Q<VisualElement>("Player3Frame"),
            this.root.Q<VisualElement>("Player4Frame")
        };

        this.m_PlayerHealth = new Label[]
        {
            this.root.Q<Label>("Player1Health"),
            this.root.Q<Label>("Player2Health"),
            this.root.Q<Label>("Player3Health"),
            this.root.Q<Label>("Player4Health")
        };
    }

    private void Start()
    {
        //this.m_Character = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>();

        //this.m_ProgressBar.lowValue = 0;
        //this.m_ProgressBar.highValue = this.m_Character.CharacterData.MaxHealth;
    }

    private void Update()
    {
        //this.m_ProgressBar.value = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.CurrHealth;
        //this.m_ProgressBar.highValue = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>().CharacterData.MaxHealth;
        //this.m_ProgressBar.value = this.m_Character.CharacterData.CurrHealth;

        this.UpdatePlayerFrames();
        this.UpdatePlayerHealth();

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
                            MusicManager.instance.RevertBGM();

                        //DiceManager.Instance.DoRoll();
                        break;

                    case "SwitchToP1":
                        PartyManager.Instance.SwitchActiveCharacterByName("Player1");
                        break;
                        
                    case "SwitchToP2":
                        PartyManager.Instance.SwitchActiveCharacterByName("Player2");
                        break;
                        
                    case "SwitchToP3":
                        PartyManager.Instance.SwitchActiveCharacterByName("Player3");
                        break;
                        
                    case "SwitchToP4":
                        PartyManager.Instance.SwitchActiveCharacterByName("Player4");
                        break;


                    default:
                        break;
                }
            }));
        }
   
    }

    private void UpdatePlayerHealth()
    {
        for (int i = 0; i < this.m_PlayerHealth.Length; i++)
        {
            GameObject m_Player = PartyManager.Instance.FindCharacterByName($"Player{i + 1}");
            this.m_PlayerHealth[i].text = $"{m_Player.GetComponent<CharacterScript>().CharacterData.CurrHealth}/{m_Player.GetComponent<CharacterScript>().CharacterData.MaxHealth}";
        }
    }

    private void UpdatePlayerFrames()
    {
        for (int i = 0; i < this.m_PlayerFrames.Length; i++)
        {
            GameObject m_Player = PartyManager.Instance.FindCharacterByName($"Player{i + 1}");

            if (m_Player != null)
                this.m_PlayerFrames[i].style.display = DisplayStyle.Flex;

            else
                this.m_PlayerFrames[i].style.display = DisplayStyle.None;
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
