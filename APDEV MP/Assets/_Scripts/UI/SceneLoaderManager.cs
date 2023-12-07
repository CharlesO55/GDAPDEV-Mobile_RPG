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

    [Header("Spawn Area in the new scene")]
    private int m_SpawnAreaIndex;
    public int SpawnAreaIndex
    {
        get { return m_SpawnAreaIndex; }
    }



    private SceneSaveData m_SceneSaveData = new();


    [Header("Use in refreshing data")]
    public bool IsNewPlayerSave;

    public EventHandler<Scene> OnLoadingScreenClose;


    public void LoadScene(int sceneId, int spawnAreaIndex = 0)
    {
        if (sceneId >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("ERROR: SceneId not within range of scene count in build settings.");
            return;
        }

        this.m_SceneSaveData.SceneIndex = sceneId;
        this.m_SceneSaveData.SpawnAreaIndex = spawnAreaIndex;
        SaveSystem.Save<SceneSaveData>(this.m_SceneSaveData, SaveSystem.SAVE_FILE_ID.SCENE_DATA);


        if(sceneId == 0)
        {
            CleanUpDontDestroys();
        }

        int currSceneID = SceneManager.GetActiveScene().buildIndex;
        this.SaveBeforeSceneChange(currSceneID);
        
        this.m_SpawnAreaIndex = spawnAreaIndex;

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
        this.m_LoadingScreen.enabled = true;

        yield return SceneManager.LoadSceneAsync(sceneId);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneId));
        
        this.OnLoadingScreenClose?.Invoke(this, SceneManager.GetActiveScene());
        
        
        while (AssetSpawner.Instance.IsSpawning)
        {
            yield return null;
        }

        this.m_LoadingScreen.enabled = false;
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
