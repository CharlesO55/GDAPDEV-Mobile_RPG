using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument _systemMessagesDocs;
    [SerializeField] private UIDocument _gameHUDDocument;

    public static UIManager Instance;
    private Label _systemMessagesLabel;
    private Label m_TurnLabel;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            Debug.LogError("Warning more than one UIManager exists");
            return;
        }
        Instance = this;
    }

    void Start()
    {
        DiceManager.Instance.OnDiceResultObservsers += DisplayDiceResults;
        GestureManager.Instance.OnTapDelegate += DisplayGestureTap;
        GestureManager.Instance.OnSwipeDelegate += DisplayGestureSwipe;

        //FIND LABEL TO MODIFY LATER
        _systemMessagesLabel = _systemMessagesDocs?.rootVisualElement.Q<Label>("Message");
        this.m_TurnLabel = this._gameHUDDocument?.rootVisualElement.Q<Label>("TurnLabel");
    }

    private void OnDisable()
    {
        DiceManager.Instance.OnDiceResultObservsers -= DisplayDiceResults;
        GestureManager.Instance.OnTapDelegate -= DisplayGestureTap;
        GestureManager.Instance.OnSwipeDelegate -= DisplayGestureSwipe;
    }

    public void ChangeText(string message)
    {
        this._systemMessagesLabel.text = message;
    }

    public void ChangeTurn(string message)
    {
        this.m_TurnLabel.text = message;
    }

    private void DisplayDiceResults(object sender, DieArgs args)
    {
        string strResult = args.RollPass ? "[Success]" : "[Failed]";     
        strResult += ": Rolled a " + args.RolledValue + '/' + args.MinValue;
        

        this.ChangeText(strResult);
    }
    private void DisplayGestureTap(object sender, TapEventArgs args)
    {
        string message = "Tap heard.";
        if(args.ObjHit != null) { message += args.ObjHit.name + " was hit";  }
        //this.ChangeText(message);
    }
    private void DisplayGestureSwipe(object sender, SwipeEventArgs args)
    {
        string message = args.Direction + " Swipe heard.";
        if (args.ObjHit != null) { message += args.ObjHit.name + " was hit"; }
        //this.ChangeText(message);
    }


    public GameHUD GetGameHUD()
    {
        GameHUD docScript = null;

        if (this._gameHUDDocument != null)
        {
            if (this._gameHUDDocument.TryGetComponent<GameHUD>(out docScript))
            {
                return docScript;
            }
        }

        Debug.LogError("UIManager Failed to find GameHUD");
        return docScript;
    }


    public void ToggleGameHUDControls(bool bEnable)
    {
        GameObject uiElements = GameObject.Find("UI Elements");
        if (uiElements != null && uiElements.TryGetComponent<Canvas>(out Canvas joystickCanvas))
        {
            joystickCanvas.enabled = bEnable;
        }
        else
        {
            Debug.LogError("Failed to toggle joystick's canvas");
        }

        this._gameHUDDocument.rootVisualElement.Q<VisualElement>("Buttons").visible = bEnable;
    }
}
