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
    private Button godModeButton;
    private Button endCombatButton;
    private Button killParty;
    private Button killEnemies;
    private Button unlockDoorsButton;

    private Label m_MoralityLevel;

    
    void Start()
    {
        this.DevMenu = this.gameObject.GetComponent<UIDocument>();
        this.root = this.DevMenu.rootVisualElement;

        this.DevOptions = this.root.Q<VisualElement>("DevOptionsContainer");

        this.autoWinButton = this.root.Q<Button>("AutoWinButton");
        this.autoLoseButton = this.root.Q<Button>("AutoLoseButton");
        this.returnButton = this.root.Q<Button>("ReturnButton");
        this.godModeButton = this.root.Q<Button>("GodModeButton");
        this.endCombatButton = this.root.Q<Button>("EndCombatButton");
        this.unlockDoorsButton = this.root.Q<Button>("UnlockDoorsButton");
        this.killParty = this.root.Q<Button>("KillPartyButton");
        this.killEnemies = this.root.Q<Button>("KillEnemiesButton");

        this.m_MoralityLevel = this.root.Q<Label>("MoralityLevel");

        this.autoWinButton.clicked += this.ToggleAutoWin;
        this.autoLoseButton.clicked += this.ToggleAutoLose;
        this.returnButton.clicked += this.ReturnToGame;
        this.godModeButton.clicked += this.ToggleGodMode;
        this.endCombatButton.clicked += this.EndCombat;
        this.unlockDoorsButton.clicked += this.ToggleDoorUnlock;
        this.killParty.clicked += this.TriggerKillParty;
        this.killEnemies.clicked += this.TriggerKillEnemies;
    }

    private void OnDestroy()
    {
        this.autoWinButton.clicked -= this.ToggleAutoWin;
        this.autoLoseButton.clicked -= this.ToggleAutoLose;
        this.returnButton.clicked -= this.ReturnToGame;
        this.godModeButton.clicked -= this.ToggleGodMode;
        this.endCombatButton.clicked -= this.EndCombat;
        this.unlockDoorsButton.clicked -= this.ToggleDoorUnlock;
        this.killParty.clicked -= this.TriggerKillParty;
        this.killEnemies.clicked -= this.TriggerKillEnemies;
    }
    private void Update()
    {
        this.m_MoralityLevel.text = "Morality: " + MultipleQuestsManager.Instance.PlayerMorality;
    }

    void PlayButtonSound()
    {
        SFXManager.Instance.PlaySFX(EnumSFX.SFX_BUTTON);
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
        PlayButtonSound();

        GameSettings.IS_DIEROLL_ALWAYS_WIN = !GameSettings.IS_DIEROLL_ALWAYS_WIN;

        this.autoWinButton.style.backgroundColor = (GameSettings.IS_DIEROLL_ALWAYS_WIN) ? Color.green : Color.red;
        
        //DEACTIVATE AUTOLOSE IF BOTH ARE ENABLED
        if (GameSettings.IS_DIEROLL_ALWAYS_WIN && GameSettings.IS_DIEROLL_ALWAYS_FAIL)
        {
            ToggleAutoLose();
        }
        
    }
    private void ToggleAutoLose()
    {
        Debug.Log("[DEVELOPER OPTIONS] : Auto Lose Toggled!");
        PlayButtonSound();


        GameSettings.IS_DIEROLL_ALWAYS_FAIL = !GameSettings.IS_DIEROLL_ALWAYS_FAIL;

        this.autoLoseButton.style.backgroundColor = (GameSettings.IS_DIEROLL_ALWAYS_FAIL) ? Color.green : Color.red;

        //DEACTIVATE AUTOWIN IF BOTH ARE ENABLED
        if (GameSettings.IS_DIEROLL_ALWAYS_WIN && GameSettings.IS_DIEROLL_ALWAYS_FAIL)
        {
            ToggleAutoWin();
        }
    }
    private void ReturnToGame()
    {
        PlayButtonSound();

        DebugginButton.isPaused = false;
        Debug.Log(DebugginButton.isPaused);
        Time.timeScale = 1;
        this.ToggleVisibility(false);
    }

    private void ToggleGodMode()
    {
        Debug.Log("[DEVELOPER OPTIONS] : God Mode Toggled!");
        PlayButtonSound();


        GameSettings.IS_GODMODE_ON = !GameSettings.IS_GODMODE_ON;
        this.godModeButton.style.backgroundColor = (GameSettings.IS_GODMODE_ON) ? Color.green : Color.red;
    }

    private void EndCombat()
    {
        PlayButtonSound();

        CombatManager.Instance.EndCombat();
    }

    private void ToggleDoorUnlock()
    {
        PlayButtonSound();

        GameSettings.IS_UNLOCK_ALL_DOORS = !GameSettings.IS_UNLOCK_ALL_DOORS;
        this.unlockDoorsButton.style.backgroundColor = (GameSettings.IS_UNLOCK_ALL_DOORS) ? Color.green : Color.red;
    }

    private void TriggerKillParty()
    {
        PlayButtonSound();

        PartyManager.Instance.UpdateStats("HP", -100, true);
    }

    private void TriggerKillEnemies()
    {
        PlayButtonSound();

    }
}
