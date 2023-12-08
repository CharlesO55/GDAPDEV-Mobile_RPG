using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance;

    private UIDocument m_LoadingScreen;




    [Header("Saved Scene Data")]
    [SerializeField] private SceneSaveData m_SceneSaveData = new();
    [HideInInspector] public int SpawnAreaIndex
    {
        get { return m_SceneSaveData.SpawnAreaIndex; }
    }


    [Header("Use in refreshing data")]
    public bool IsNewPlayerSave;
    public bool IsPlayerDefeated { get; private set; }

    public EventHandler<Scene> OnLoadingScreenClose;
    

    public void LoadScene(int sceneId, int spawnAreaIndex = 0, bool isPlayerDefeated = false)
    {
        IsPlayerDefeated = isPlayerDefeated;
        if (sceneId >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("ERROR: SceneId not within range of scene count in build settings.");
            return;
        }

        //SAVE NEXT SCENE DETAILS
        this.m_SceneSaveData.SceneIndex = sceneId;
        this.m_SceneSaveData.SpawnAreaIndex = spawnAreaIndex;
        SaveSystem.Save<SceneSaveData>(this.m_SceneSaveData, SaveSystem.SAVE_FILE_ID.SCENE_DATA);

        //PREPARE FOR NEW GAME
        if(sceneId == 0)
        {
            CleanUpDontDestroys();
        }

        
        this.SaveBeforeSceneChange(SceneManager.GetActiveScene().buildIndex);
        

        this.StartCoroutine(this.ShowLoadingScreen(sceneId));    
    }

    void SaveBeforeSceneChange(int currSceneID)
    {
        //DON'T SAVE AT TITLE AND END SCREEN
        if(currSceneID != 0 && 
            currSceneID != GameSettings.PLAYABLE_SCENES_INDEX_RANGE.Item2 + 1)
        {
            Debug.Log("Saving before scene change");
            PartyManager.Instance.SavePartyData(false);
        }
    }

    private void CleanUpDontDestroys()
    {
        if(PartyManager.Instance != null) { Destroy(PartyManager.Instance.gameObject); }
        if(MultipleQuestsManager.Instance != null) { Destroy(MultipleQuestsManager.Instance.gameObject); }
    }


    private IEnumerator ShowLoadingScreen(int sceneId)
    {
        this.ToggleLoadingScreen(true);

        yield return SceneManager.LoadSceneAsync(sceneId);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneId));
        
        this.OnLoadingScreenClose?.Invoke(this, SceneManager.GetActiveScene());
        
        if(sceneId < GameSettings.PLAYABLE_SCENES_INDEX_RANGE.Item1 || 
            sceneId > GameSettings.PLAYABLE_SCENES_INDEX_RANGE.Item2)
        {
            //TURN OFF WHEN THERE'S NO ASSETSPAWNER
            this.ToggleLoadingScreen(false);
        }
        else
        {
            //LOADING SCREEN IS DISABLED BY ASSETSPAWNER
            //WHEN ALL ASEETS ARE ALREADY SPAWNED
        }
    }

    public void ToggleLoadingScreen(bool bEnable)
    {
        this.m_LoadingScreen.enabled = bEnable;
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
