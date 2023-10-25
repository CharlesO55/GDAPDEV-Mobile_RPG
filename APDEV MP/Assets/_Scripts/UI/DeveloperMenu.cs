using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class DeveloperMenu : MonoBehaviour
{
    private UIDocument DevMenu;
    private VisualElement root;
    private Button autoWinButton;
    private Button autoLoseButton;
    private Button returnButton;
    public static bool isAutoWin = false;
    public static bool isAutoLose = false;
    // Start is called before the first frame update
    void Start()
    {
        this.DevMenu = this.gameObject.GetComponent<UIDocument>();
        this.root = this.DevMenu.rootVisualElement;
        this.autoWinButton = this.root.Q<Button>("AutoWinButton");
        this.autoLoseButton = this.root.Q<Button>("AutoLoseButton");
        this.returnButton = this.root.Q<Button>("ReturnButton");

        this.autoWinButton.clicked += this.ToggleAutoWin;
        this.autoLoseButton.clicked += this.ToggleAutoLose;
        this.returnButton.clicked += this.ReturnToGame;


    }

    private void ToggleAutoWin()
    {
        if (!isAutoWin)
        {
            isAutoWin = !isAutoWin;
            this.autoWinButton.style.backgroundColor = Color.green;
        }
            
        else
        {
            isAutoWin = !isAutoWin;
            this.autoWinButton.style.backgroundColor = Color.red;
        }
    }
    private void ToggleAutoLose()
    {
        if(!isAutoLose)
        {
            isAutoLose = !isAutoLose;
           autoLoseButton.style.backgroundColor = Color.green;
        }
        else
        {
            isAutoLose = !isAutoLose;
            autoLoseButton.style.backgroundColor = Color.red;
        }
    }
    private void ReturnToGame()
    {   
        DebugginButton.isPaused = false;
        Debug.Log(DebugginButton.isPaused);
        Time.timeScale = 1;
        this.DevMenu.enabled = false;
    }

}
