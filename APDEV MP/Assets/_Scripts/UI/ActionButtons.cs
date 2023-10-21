using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionButtons : MonoBehaviour
{
    private VisualElement root;

    private Image m_Interact;
    private Image m_Attack;

    void OnEnable()
    {
        this.root = GetComponent<UIDocument>().rootVisualElement;

        this.m_Interact = this.root.Q<Image>("InteractButton");
        this.m_Attack = this.root.Q<Image>("AttackButton");

        this.ClickedImage(this.m_Interact, "Interact");
        this.ClickedImage(this.m_Attack, "Attack");
    }

    private void ClickedImage(Image img, string eventname)
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
