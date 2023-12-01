using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance;

    private UIDocument m_LoadingScreen;

    [Header("Spawn Area in the new scene")]
    private int m_SpawnAreaIndex;
    public int SpawnAreaIndex
    {
        get { return m_SpawnAreaIndex; }
    }

    [Header("Use in refreshing data")]
    private bool m_IsNewGame;
    public bool IsNewGame
    {
        get { return m_IsNewGame; }
    }

    public void LoadScene(int sceneId, int spawnAreaIndex = 0)
    {
        if (sceneId >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("ERROR: SceneId not within range of scene count in build settings.");
            return;
        }


        this.CheckSaveAction(sceneId, SceneManager.GetActiveScene().buildIndex);

        this.m_SpawnAreaIndex = spawnAreaIndex;
        this.StartCoroutine(this.ShowLoadingScreen(sceneId));
    }


    private void CheckSaveAction(int targetScene, int currScene)
    {
        //DO NOT SAVE WHEN ACTIVE SCENE IS TITLE / END
        switch (currScene)
        {
            case 0:
            case 7:
                break;
            default:
                CallSaves();
                break;
        }

        //DO NOT SAVE WHEN TARGET SCENE IS TITLE/END
        switch (targetScene)
        {
            case 0:
            case 7:
                this.m_IsNewGame = true;
                break;
            default:
                CallSaves();
                break;
        }
    }

    private void CallSaves()
    {
        if (PartyManager.Instance != null)
        {
            PartyManager.Instance.SavePartyData();
        }
    }


    private IEnumerator ShowLoadingScreen(int sceneId)
    {
        this.m_LoadingScreen.enabled = true;

        yield return SceneManager.LoadSceneAsync(sceneId);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneId));
        this.m_LoadingScreen.enabled = false;
    }

    public void ToggleNewGame(bool ToF)
    {
        this.m_IsNewGame = ToF;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
            this.m_LoadingScreen = GetComponent<UIDocument>();

            NotificationManager.SendNotification("Start", "Start your journey!");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
