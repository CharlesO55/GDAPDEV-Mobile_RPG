using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance;

    private UIDocument m_LoadingScreen;

    private int m_SpawnAreaIndex;
    public int SpawnAreaIndex
    {
        get { return m_SpawnAreaIndex; }
    }

    public void LoadScene(int sceneId, int spawnAreaIndex = 0)
    {
        if (sceneId >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("ERROR: SceneId not within range of scene count in build settings.");
        }
        else
        {
            this.m_SpawnAreaIndex = spawnAreaIndex;
            this.StartCoroutine(this.ShowLoadingScreen(sceneId));
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
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
