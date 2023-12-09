using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndingSceneScript : MonoBehaviour
{
    private VisualElement _root;
    private Image m_Background;
    private Button _returnToMain;

    [SerializeField] private Sprite m_GoodScreen;
    [SerializeField] private Sprite m_NeutralScreen;
    [SerializeField] private Sprite m_BadScreen;
    [SerializeField] private Sprite m_DeadScreen;

    
    // Start is called before the first frame update
    void Start()
    {
        this._root = this.GetComponent<UIDocument>().rootVisualElement;
        this.m_Background = this._root.Q<Image>("BackgroundArt");
        this._returnToMain = this._root.Q<Button>("MenuReturn");


        if(SceneLoaderManager.Instance.IsPlayerDefeated)
        {
            this.DeadEnd();
        }
        else
        {
            if (MultipleQuestsManager.Instance.PlayerMorality > 2) this.GoodEnd();
            else if (MultipleQuestsManager.Instance.PlayerMorality < -2) this.BadEnd();
            else this.NeutralEnd();
        }
        this._returnToMain.clicked += ReturnToMain;

    }
    private void ReturnToMain()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    private void GoodEnd()
    {
        this.m_Background.sprite = this.m_GoodScreen;
        SFXManager.Instance.PlaySFX(EnumSFX.SFX_ENDING_DEFAULT);
    }
    private void NeutralEnd()
    {
        this.m_Background.sprite = this.m_NeutralScreen;
        SFXManager.Instance.PlaySFX(EnumSFX.SFX_ENDING_DEFAULT);
    }
    private void BadEnd()
    {
        this.m_Background.sprite = this.m_BadScreen;
        SFXManager.Instance.PlaySFX(EnumSFX.SFX_ENDING_DEFAULT);
    }
    private void DeadEnd()
    {
        this.m_Background.sprite = this.m_DeadScreen;
        SFXManager.Instance.PlaySFX(EnumSFX.SFX_ENDING_DEAD);
    }
}
