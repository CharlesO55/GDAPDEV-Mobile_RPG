using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private List<AssetLabelReference> _startingSceneLabels;

    private VisualElement _root;
    private Button _start;
    private Button _continue;
    private Button _quit;
    // Start is called before the first frame update
    void Start()
    {
        this._root = this.GetComponent<UIDocument>().rootVisualElement;
        this._start = this._root.Q<Button>("GameBegin");
        this._continue = this._root.Q<Button>("GameContinue");
        this._quit = this._root.Q<Button>("GameQuit");

        this._start.clicked += this.StartGame;
        this._continue.clicked += this.LoadGame;
        this._quit.clicked += this.QuitGame;
    }

    private void OnDestroy()
    {
        this._start.clicked -= this.StartGame;
        this._continue.clicked -= this.LoadGame;
        this._quit.clicked -= this.QuitGame;
    }

    private void StartGame()
    {
        AssetSpawner.Instance.MarkNextSceneAssets(_startingSceneLabels);
        SceneLoaderManager.Instance.IsNewPlayerSave = true;
        SceneLoaderManager.Instance.LoadScene(1);
    }

    private void LoadGame()
    {
        SceneSaveData lastSceneSaved = SaveSystem.LoadSingle<SceneSaveData>(SaveSystem.SAVE_FILE_ID.SCENE_DATA);
        
        if(lastSceneSaved == null)
        {
            Debug.LogError("There is no save data for Last Scene Opened");
            //START AS NEW GAME INSTEAD
            this.StartGame();
        }
        else
        {
            AssetSpawner.Instance.MarkNextSceneAssets(
                SaveSystem.LoadSingle<SceneSaveData>(SaveSystem.SAVE_FILE_ID.SCENE_DATA).SceneAssetLabels
            );
            SceneLoaderManager.Instance.LoadScene(lastSceneSaved.SceneIndex, lastSceneSaved.SpawnAreaIndex);
        }
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
