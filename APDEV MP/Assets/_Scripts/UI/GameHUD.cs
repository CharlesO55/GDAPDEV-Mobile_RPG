using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour
{
    private VisualElement m_Root;


    private Image m_Interact;
    private Image m_Attack;

    private Image m_Player1Portrait;
    private Image m_Player2Portrait;
    private Image m_Player3Portrait;
    private Image m_Player4Portrait;

    private Image m_CombatMoveButton;
    private Image m_CombatAttackButton;
    private Image m_CombatEndTurnButton;

    private VisualElement[] m_PlayerFrames;
    private Label[] m_PlayerHealth;

    private Label m_DiceRollLabel;

    private Label[] m_QuestLabels;
    private Label[] m_TaskLabels;

    private VisualElement m_TurnIndicator;

    private VisualElement m_ButtonContainer;
    private VisualElement m_CombatButtonContainer;
    private VisualElement m_QuestContainer;

    [SerializeField] private AudioClip BattleBgm;
    [SerializeField] private GameObject m_Joystick;
    [SerializeField] private CombatGrid m_CombatGrid;

    void OnEnable()
    {
        this.m_Root = GetComponent<UIDocument>().rootVisualElement;

        this.m_Interact = this.m_Root.Q<Image>("InteractButton");
        this.m_Attack = this.m_Root.Q<Image>("AttackButton");

        this.m_Player1Portrait = this.m_Root.Q<Image>("Player1Image");
        this.m_Player2Portrait = this.m_Root.Q<Image>("Player2Image");
        this.m_Player3Portrait = this.m_Root.Q<Image>("Player3Image");
        this.m_Player4Portrait = this.m_Root.Q<Image>("Player4Image");

        this.m_CombatMoveButton = this.m_Root.Q<Image>("CombatMoveButton");
        this.m_CombatAttackButton = this.m_Root.Q<Image>("CombatAttackButton");
        this.m_CombatEndTurnButton = this.m_Root.Q<Image>("CombatEndButton");

        this.m_DiceRollLabel = this.m_Root.Q<Label>("DiceRollLabel");

        this.m_TurnIndicator = this.m_Root.Q<VisualElement>("TurnIndicator");

        this.m_ButtonContainer = this.m_Root.Q<VisualElement>("Buttons");
        this.m_CombatButtonContainer = this.m_Root.Q<VisualElement>("CombatButtons");
        this.m_QuestContainer = this.m_Root.Q<VisualElement>("QuestStuff");

        this.ClickedImage(this.m_Interact, "Interact");
        this.ClickedImage(this.m_Attack, "Attack");

        this.ClickedImage(this.m_Player1Portrait, "SwitchToP1");
        this.ClickedImage(this.m_Player2Portrait, "SwitchToP2");
        this.ClickedImage(this.m_Player3Portrait, "SwitchToP3");
        this.ClickedImage(this.m_Player4Portrait, "SwitchToP4");

        this.ClickedImage(this.m_CombatMoveButton, "ShowMove");
        this.ClickedImage(this.m_CombatAttackButton, "ShowAttack");
        this.ClickedImage(this.m_CombatEndTurnButton, "EndTurn");

        //this.m_QuestLabel = this.m_Root.Q<Label>("QuestLabel");
        //this.m_TaskLabel = this.m_Root.Q<Label>("TaskLabel");

        this.LinkQuestUI();

        this.m_PlayerFrames = new VisualElement[]
        {
            this.m_Root.Q<VisualElement>("Player1Frame"),
            this.m_Root.Q<VisualElement>("Player2Frame"),
            this.m_Root.Q<VisualElement>("Player3Frame"),
            this.m_Root.Q<VisualElement>("Player4Frame")
        };

        this.m_PlayerHealth = new Label[]
        {
            this.m_Root.Q<Label>("Player1Health"),
            this.m_Root.Q<Label>("Player2Health"),
            this.m_Root.Q<Label>("Player3Health"),
            this.m_Root.Q<Label>("Player4Health")
        };
    }


    private void LinkQuestUI()
    {
        m_QuestLabels = new Label[]{
            this.m_Root.Q<Label>("QuestLabel"),
            this.m_Root.Q<Label>("QuestLabel2"),
            this.m_Root.Q<Label>("QuestLabel3")
        };

        m_TaskLabels = new Label[]{
            this.m_Root.Q<Label>("TaskLabel"),
            this.m_Root.Q<Label>("TaskLabel2"),
            this.m_Root.Q<Label>("TaskLabel3")
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

        this.ToggleUI();
        this.UpdatePlayerFrames();
        this.UpdatePlayerHealth();

        if (DialogueManager.Instance.IsRequestingRoll || CombatManager.Instance.IsRequestingRoll)
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

                        //DiceManager.Instance.DoRoll();
                        break;

                    case "ShowMove":
                        this.m_CombatGrid.ResetGrid();
                        CombatManager.Instance.IsViewingMoveRange = true;
                        CombatManager.Instance.IsViewingAttackRange = false;
                        break;

                    case "ShowAttack":
                        this.m_CombatGrid.ResetGrid();
                        CombatManager.Instance.IsViewingMoveRange = false;
                        CombatManager.Instance.IsViewingAttackRange = true;
                        break;

                    case "EndTurn":
                        CombatManager.Instance.EndTurn();
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

    private void ToggleUI()
    {
        if (CombatManager.Instance.IsInCombat)
        {
            this.m_QuestContainer.style.display = DisplayStyle.None;
            this.m_ButtonContainer.style.display = DisplayStyle.None;
            this.m_TurnIndicator.style.display = DisplayStyle.Flex;

            if (CombatManager.Instance.IsEnemyTurn)
                this.m_CombatButtonContainer.style.display = DisplayStyle.None;

            else
                this.m_CombatButtonContainer.style.display = DisplayStyle.Flex;

            this.m_Joystick.SetActive(false);
        }

        else
        {
            this.m_QuestContainer.style.display = DisplayStyle.Flex;
            this.m_ButtonContainer.style.display = DisplayStyle.Flex;
            this.m_TurnIndicator.style.display = DisplayStyle.None;
            this.m_CombatButtonContainer.style.display = DisplayStyle.None;
            this.m_Joystick.SetActive(true);
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


    public void ToggleRerollUI(bool bEnable)
    {
        VisualElement rerollRoot = this.m_Root.Q<VisualElement>("RerollScreen");
        Button rerollYes = rerollRoot.Q<Button>("RerollYesButton");
        Button rerollNo = rerollRoot.Q<Button>("RerollNoButton");


        if (bEnable)
        {
            rerollRoot.style.display = DisplayStyle.Flex;
            rerollYes.clicked += AdManager.Instance.ShowAd;

            rerollYes.clicked += CloseRerollUI;
            rerollNo.clicked += CloseRerollUI;
            rerollNo.clicked += DiceManager.Instance.BroadcastResult;

            this.ToggleControlsUI(false);
        }
        else
        {
            rerollYes.clicked -= AdManager.Instance.ShowAd;
            rerollYes.clicked -= CloseRerollUI;
            rerollNo.clicked -= CloseRerollUI;
            rerollNo.clicked -= DiceManager.Instance.BroadcastResult;

            rerollRoot.style.display = DisplayStyle.None;

            this.ToggleControlsUI(true);
        }
    }


    private void CloseRerollUI()
    {
        ToggleRerollUI(false);
    }

    private void ToggleControlsUI(bool bVisible)
    {
        DisplayStyle display = (bVisible) ? DisplayStyle.Flex : DisplayStyle.None;


        this.m_Interact.style.display = display;
        this.m_Attack.style.display = display;

        
        this.m_Joystick.GetComponentInParent<Canvas>().enabled = bVisible;
    }

}
