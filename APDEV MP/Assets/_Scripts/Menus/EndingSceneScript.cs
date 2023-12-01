using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndingSceneScript : MonoBehaviour
{
    public static bool isDead;
    private VisualElement _root;
    private Button _returnToMain;

    private Label _endingText;
    private Label _EndingType;

    private string _goodEndText = "As the dust of the battle settles the hero party rejoices and returns to Home to be showered by \r\npraise and admiration of the people for their deeds in the final battle against the demon king. as time passes \r\nthey are remembered for their acts of valor that helped in shaping the world in the future.";
    private string _neutralEndText = "The heroes have defeated the demon king but that is all they are known for without helping others they are only\r\nremembered for the feat of defeating the demon king. As time passes that feat is buried under the hundreds of other parties\r\nthat have done similar fantastic feats and they are forgotten to time.";
    private string _badEndText = "The Demon King's head falls to the floor as the new rulers of the demon armies rise to take his place the hero party\r\nnow known as the Four Demonic Lords begin their conquest of humankind and other nations. With the nations of old buried under\r\nthe might of the Four Demonic Lords they began to fight among themselves beginning an era long war that ended in mutal \r\ndestruction.";


    // Start is called before the first frame update
    void Start()
    {
        this._root = this.GetComponent<UIDocument>().rootVisualElement;
        this._endingText = this._root.Q<Label>("EndingText");
        this._EndingType = this._root.Q<Label>("EndingType");
        this._returnToMain = this._root.Q<Button>("MenuReturn");

        if(MultipleQuestsManager.Instance.PlayerMorality > -2 && MultipleQuestsManager.Instance.PlayerMorality < 2 && !isDead)
        {
            this.NeutralEnd();
        }
        else if(MultipleQuestsManager.Instance.PlayerMorality > 2 && !isDead)
        {
            this.GoodEnd();
        }
        else if(MultipleQuestsManager.Instance.PlayerMorality < -2 && !isDead)
        {
             this.BadEnd();
        }

        if(isDead)
        {
            this.DeadEnd();
        }
        this._returnToMain.clicked += ReturnToMain;
        
    }
    private void ReturnToMain()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    private void GoodEnd()
    {
        this._endingText.text = _goodEndText;
        this._EndingType.text = "Good Ending";
        this._EndingType.style.color = Color.green;
    }
    private void NeutralEnd()
    {
        this._endingText.text = _neutralEndText;
        this._EndingType.text = "Neutral Ending";
        this._EndingType.style.color = Color.blue;
    }
    private void BadEnd()
    {
        this._endingText.text = _badEndText;
        this._EndingType.text = "Bad Ending";
        this._EndingType.style.color = Color.red;
    }
    private void DeadEnd()
    {
        this._endingText.text = "You have Died";
        this._EndingType.text = "Dead Ending";
        this._EndingType.style.color = Color.red;
    }
}
