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

    void OnEnable()
    {
        this.root = GetComponent<UIDocument>().rootVisualElement;
        this.m_ProgressBar = this.root.Q<ProgressBar>("Health");

        this.m_Interact = this.root.Q<Image>("InteractButton");
        this.m_Attack = this.root.Q<Image>("AttackButton");

        this.m_DiceRollLabel = this.root.Q<Label>("DiceRollLabel");

        this.ClickedImage(this.m_Interact, "Interact");
        this.ClickedImage(this.m_Attack, "Attack");
    }

    private void Start()
    {
        this.m_Character = PartyManager.Instance.ActivePlayer.GetComponent<CharacterScript>();

        this.m_ProgressBar.lowValue = 0;
        this.m_ProgressBar.highValue = this.m_Character.CharacterData.MaxHealth;
    }

    private void Update()
    {
        this.m_ProgressBar.value = this.m_Character.CharacterData.CurrHealth;

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
            Debug.Log(" sddddddddddddddddd"  + eventname);

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
                        //DiceManager.Instance.DoRoll();
                        break;

                    default:
                        break;
                }
            }));
        }
   
    }
}
