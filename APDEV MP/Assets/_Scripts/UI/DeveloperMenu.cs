using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class DeveloperMenu : MonoBehaviour
{
    private UIDocument DevMenu;

    private VisualElement root;
    private VisualElement DevOptions;

    private Button autoWinButton;
    private Button autoLoseButton;
    private Button returnButton;

    private Label m_MoralityLevel;

    public static bool isAutoWin = false;
    public static bool isAutoLose = false;
    // Start is called before the first frame update
    void Start()
    {
        this.DevMenu = this.gameObject.GetComponent<UIDocument>();
        this.root = this.DevMenu.rootVisualElement;

        this.DevOptions = this.root.Q<VisualElement>("DevOptionsContainer");

        this.autoWinButton = this.root.Q<Button>("AutoWinButton");
        this.autoLoseButton = this.root.Q<Button>("AutoLoseButton");
        this.returnButton = this.root.Q<Button>("ReturnButton");

        this.m_MoralityLevel = this.root.Q<Label>("MoralityLevel");

        this.autoWinButton.clicked += this.ToggleAutoWin;
        this.autoLoseButton.clicked += this.ToggleAutoLose;
        this.returnButton.clicked += this.ReturnToGame;
    }

    private void Update()
    {
        this.m_MoralityLevel.text = "Morality: " + MultipleQuestsManager.Instance.PlayerMorality;//QuestManager.Instance.PlayerMorality;
    }

    public void ToggleVisibility(bool isVisible)
    {
        if (isVisible)
            this.DevOptions.style.display = DisplayStyle.Flex;

        else
            this.DevOptions.style.display = DisplayStyle.None;
    }

    private void ToggleAutoWin()
    {
        Debug.Log("[DEVELOPER OPTIONS] : Auto Win Toggled!");

        if (!isAutoWin)
        {
            isAutoWin = true;
            DiceManager.Instance.IsAlwaysWin = true;
            this.autoWinButton.style.backgroundColor = Color.green;

            if (isAutoLose)
            {
                this.ToggleAutoLose();
            }
        }
 
        else
        {
            isAutoWin = false;
            DiceManager.Instance.IsAlwaysWin = false;
            this.autoWinButton.style.backgroundColor = Color.red;
        }
    }
    private void ToggleAutoLose()
    {
        Debug.Log("[DEVELOPER OPTIONS] : Auto Lose Toggled!");

        if (!isAutoLose)
        {
            isAutoLose = true;
            DiceManager.Instance.IsAlwaysLoss = true;
            this.autoLoseButton.style.backgroundColor = Color.green;

            if (isAutoWin)
            {
                this.ToggleAutoWin();
            }
        }

        else
        {
            isAutoLose = false;
            DiceManager.Instance.IsAlwaysLoss = false;
            this.autoLoseButton.style.backgroundColor = Color.red;
        }
    }
    private void ReturnToGame()
    {   
        DebugginButton.isPaused = false;
        Debug.Log(DebugginButton.isPaused);
        Time.timeScale = 1;
        this.ToggleVisibility(false);
    }

}
