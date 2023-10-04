using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument _systemMessagesDocs;
    private Label _systemMessagesLabel;


    void Start()
    {
        DiceManager.Instance.OnDiceResultObservsers += DisplayDiceResults;

        _systemMessagesLabel = _systemMessagesDocs?.rootVisualElement.Q<Label>("Message");
    }

    private void OnDisable()
    {
        DiceManager.Instance.OnDiceResultObservsers -= DisplayDiceResults;
    }

    private void DisplayDiceResults(object sender, DieArgs args)
    {
        string strResult = args.RollPass ? "[Success]" : "[Failed]";     
        strResult += ": Rolled a " + args.RolledValue + '/' + args.MinValue;
        

        _systemMessagesLabel.text = strResult;
    }
}
