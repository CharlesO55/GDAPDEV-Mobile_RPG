using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private VisualElement _root;
    private Button _start;
    private Button _quit;
    // Start is called before the first frame update
    void Start()
    {
        this._root = this.GetComponent<UIDocument>().rootVisualElement;
        this._start = this._root.Q<Button>("GameBegin");
        this._quit = this._root.Q<Button>("GameQuit");

        this._start.clicked += this.StartGame;
        this._quit.clicked += this.QuitGame;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Holden");
    }
    private void QuitGame()
    {
        Application.Quit();
    }
}
