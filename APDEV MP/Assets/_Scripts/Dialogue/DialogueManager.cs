using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    //UI Dialogue
    [SerializeField] private UIDocument _dialogueUI;
    private Label _dialogueTextLabel;
    private List<Button> _choiceButtons;


    private bool _isTextPrinting = false;
    private Story _currStory;
    private bool _isStoryPlaying;
    public bool IsStoryPlaying
    {
        get { return _isStoryPlaying; }
    }
    private float _dialogueWaitTime = 0.5f;
    private float _dialogueCurrWaitTimer = 0;

    private bool m_IsRequestingRoll = false;
    public bool IsRequestingRoll
    {
        get { return this.m_IsRequestingRoll; }
        set { this.m_IsRequestingRoll = value;}
    }

    private int m_MinRollValue;
    public int MinRollValue
    {
        get { return this.m_MinRollValue; }
        set { this.m_MinRollValue = value; }
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    

    private void Start()
    {
        _currStory = null;
        _isStoryPlaying = false;

        _dialogueUI.enabled = false;
        this._choiceButtons = new List<Button>();
    }

    private void Update()
    {
        _dialogueCurrWaitTimer += Time.deltaTime;
    }

    public void StartDialogue(TextAsset inkJSON, int nQuestStep = -1)
    {
        if(_isStoryPlaying)
        {
            Debug.LogWarning("Story is active");
            return;
        }

        
        this._isStoryPlaying = true;

        //Helps setup the story to select the correct segment
        this._currStory = new Story(inkJSON.text);
        if (nQuestStep >= 0)
        {
            _currStory.variablesState["_questStep"] = nQuestStep;
        }


        //Linking observers
        GestureManager.Instance.OnTapDelegate += ContinueDialogue;
        this._currStory.BindExternalFunction("SetNextStep", (int nNextStep) =>
        {
            QuestManager.Instance.SetNextStep(nNextStep);
        });
        this._currStory.BindExternalFunction("DoDialogueRoll", (int nStatRequired, string strStatType) =>
        {
            this.DoDialogueRoll(nStatRequired, strStatType);
        });


        this._dialogueUI.enabled = true;
        this.RelinkUIDocumment();   //Necessary whenver a UIDoc is enabled/disabled

        this.ContinueDialogue(null, null);
    }


    private void DoDialogueRoll(int nStatRequired, string strStatType)
    {
        int nPlayerStat = 0;

        if(PartyManager.Instance.ActivePlayer.TryGetComponent<CharacterScript>(out CharacterScript charData)){
            switch (strStatType)
            {
                case "INT":
                    nPlayerStat = charData.CharacterData.INTMod;
                    break;
                case "STR":
                    nPlayerStat = charData.CharacterData.STRMod;
                    break;
                case "CON":
                    nPlayerStat = charData.CharacterData.CONMod;
                    break;
                case "CHA":
                    nPlayerStat = charData.CharacterData.CHAMod;
                    break;
                case "DEX":
                    nPlayerStat = charData.CharacterData.DEXMod;
                    break;
                case "WIS":
                    nPlayerStat = charData.CharacterData.WISMod;
                    break;
                default:
                    Debug.LogWarning("DoDialogueRoll Error. Failed to find matching stats string from Ink");
                    break;
            }
        }

        Debug.Log("Dialogue Roll " + strStatType + "Base: " + nStatRequired + " - Player: " + nPlayerStat);

        //Disable story taps until dice manager finishes
        GestureManager.Instance.OnTapDelegate -= ContinueDialogue;

        //Link to the dice manager
        DiceManager.Instance.OnDiceResultObservsers += WaitForDieResult;
        this.m_MinRollValue = nStatRequired - nPlayerStat;
        this.m_IsRequestingRoll = true;

        //DiceManager.Instance.DoRoll(false, nStatRequired - nPlayerStat);
        
    }

    private void WaitForDieResult(object sender, DieArgs args)
    {
        _currStory.variablesState["_rollSuccess"] = args.RollPass;

        DiceManager.Instance.OnDiceResultObservsers -= WaitForDieResult;

        //Reneable the tap to continue story
        GestureManager.Instance.OnTapDelegate += ContinueDialogue;
    }

    private void ContinueDialogue(object sender, TapEventArgs args)
    {
        if (_dialogueCurrWaitTimer < _dialogueWaitTime)
        {
            //Cooldown between dialogue taps
            return;
        }
        _dialogueCurrWaitTimer = 0; //Reset
        

        //this.StopCoroutine(PrintText(null));

        if (_currStory.canContinue )
        {
            if (!_isTextPrinting)
            {
                string textToPrint = _currStory.Continue();

                StartCoroutine(PrintText(textToPrint));

                this.PrintChoices();
            }
        }
        else
        {
            this.EndDialogue();
        }
    }

    private void EndDialogue()
    {
        GestureManager.Instance.OnTapDelegate -= ContinueDialogue;
        this._isStoryPlaying = false;

        this._dialogueUI.enabled = false;

        this._currStory.UnbindExternalFunction("SetNextStep");
        this._currStory.UnbindExternalFunction("DoDialogueRoll");
    }

    private void MakeChoice(ClickEvent args)
    {
        int buttonIndex = 0;
        for (buttonIndex = 0; buttonIndex < this._choiceButtons.Count; buttonIndex++)
        {
            if (this._choiceButtons[buttonIndex] == args.target)
            {
                break;
            }
        }

        _currStory.ChooseChoiceIndex(buttonIndex);
        

        //Clean up
        foreach(Button button in this._choiceButtons)
        {
            button.UnregisterCallback<ClickEvent>(MakeChoice);
        }
        //Reset to allow story to continue
        GestureManager.Instance.OnTapDelegate += this.ContinueDialogue;

        //Continue the story as well when choice is made
        this.ContinueDialogue(null, null);
    }

    IEnumerator PrintText(string TextToPrint)
    {
        this._isTextPrinting = true;
        this._dialogueTextLabel.text = "";

        /*for (int i = 0; i < TextToPrint.Length; i++)
        {
            this._dialogueTextLabel.text += TextToPrint[i];
            yield return null;
        }*/

        int i = 0;
        while (i < TextToPrint.Length)
        {
            this._dialogueTextLabel.text += TextToPrint[i];
            i++;
            yield return null;
        }

        _isTextPrinting = false;
    }

    private void PrintChoices()
    {
        List<Choice> choices = _currStory.currentChoices;

        if(choices.Count > 0)
        {
            GestureManager.Instance.OnTapDelegate -= ContinueDialogue;

            for(int i = 0; i < choices.Count; i++)
            {
                this._choiceButtons[i].text = choices[i].text;
                //this._choiceButtons[i].visible = true;
                this._choiceButtons[i].style.display = DisplayStyle.Flex;
                this._choiceButtons[i].RegisterCallback<ClickEvent>(MakeChoice);
            }
        }

        for (int i = choices.Count; i < this._choiceButtons.Count; i++)
        {
            //this._choiceButtons[i].visible = false;
            this._choiceButtons[i].style.display = DisplayStyle.None;

        }
    }
    private void RelinkUIDocumment()
    {
        this._dialogueTextLabel = _dialogueUI.rootVisualElement.Q<Label>("DialogueText");
        
        this._choiceButtons.Clear();

        this._choiceButtons.Add(_dialogueUI.rootVisualElement.Q<Button>("Choice1"));
        this._choiceButtons.Add(_dialogueUI.rootVisualElement.Q<Button>("Choice2"));
        this._choiceButtons.Add(_dialogueUI.rootVisualElement.Q<Button>("Choice3"));
    }
}
