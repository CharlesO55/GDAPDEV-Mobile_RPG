using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument _systemMessagesDocs;
    private Label _systemMessagesLabel;


    void Start()
    {
        DiceManager.Instance.OnDiceResultObservsers += DisplayDiceResults;
        GestureManager.Instance.OnTapDelegate += DisplayGestureTap;
        GestureManager.Instance.OnSwipeDelegate += DisplayGestureSwipe;

        //FIND LABEL TO MODIFY LATER
        _systemMessagesLabel = _systemMessagesDocs?.rootVisualElement.Q<Label>("Message");
    }

    private void OnDisable()
    {
        DiceManager.Instance.OnDiceResultObservsers -= DisplayDiceResults;
        GestureManager.Instance.OnTapDelegate -= DisplayGestureTap;
        GestureManager.Instance.OnSwipeDelegate -= DisplayGestureSwipe;
    }

    private void ChangeText(string message)
    {
        this._systemMessagesLabel.text = message;
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
        if(args.ObjHit != null) { message += " Tappable " + args.ObjHit.name + " was hit";  }
        this.ChangeText(message);
    }
    private void DisplayGestureSwipe(object sender, SwipeEventArgs args)
    {
        string message = args.Direction + " Swipe heard.";
        if (args.ObjHit != null) { message += " Swipeable " + args.ObjHit.name + " was hit"; }
        this.ChangeText(message);
    }

}
