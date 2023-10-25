using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour, ITappable
{
    private CharacterScript m_Character;

    private VisualElement root;
    private ProgressBar m_ProgressBar;

    private Image m_Interact;
    private Image m_Attack;

    void OnEnable()
    {
        this.root = GetComponent<UIDocument>().rootVisualElement;
        this.m_ProgressBar = this.root.Q<ProgressBar>("Health");

        this.m_Interact = this.root.Q<Image>("InteractButton");
        this.m_Attack = this.root.Q<Image>("AttackButton");

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
    }

    public void OnTapInterface(TapEventArgs args)
    {

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
                        break;

                    case "Attack":
                        Debug.Log("Attack Button Pressed");
                        break;

                    default:
                        break;
                }
            }));
        }
   
    }
}
