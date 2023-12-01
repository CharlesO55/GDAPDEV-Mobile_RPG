using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public bool IsNewPlayerSave;
    

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
        //SAVE FROM TEMPLATE WHEN NEW GAME
        if (currScene == 0 && targetScene > 0)
        {
            IsNewPlayerSave = true;
        }
        //DO REGULAR SAVES WHEN SWITCHING BETWEEN SCENES EXCEPT RESULT
        else if (targetScene != 7)
        {
            PartyManager.Instance.SavePartyData(false);
        }
    }


    private IEnumerator ShowLoadingScreen(int sceneId)
    {
        this.m_LoadingScreen.enabled = true;

        yield return SceneManager.LoadSceneAsync(sceneId);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneId));
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
