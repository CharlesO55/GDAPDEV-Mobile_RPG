using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class AssetSpawner : MonoBehaviour
{
    public static AssetSpawner Instance;

    [SerializeField] private List<AssetLabelReference> _labels;

    private readonly string SPAWN_HOLDER_TRANSFORM_NAME = "AddresableSpawnedItems";
    private Transform _spawnHolder;

    //public bool IsSpawning { get; private set; }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SceneLoaderManager.Instance.OnLoadingScreenClose += TriggerSpawnSceneObjects;

        //TESTING
        //this.TriggerSpawnSceneObjects(this, SceneManager.GetActiveScene());
    }
    private void OnDestroy()
    {
        SceneLoaderManager.Instance.OnLoadingScreenClose -= TriggerSpawnSceneObjects;
    }

    public void MarkNextSceneAssets(List<AssetLabelReference> nextSceneLabels)
    {
        this._labels = nextSceneLabels;
    }

    public void DespawnObjects()
    {
        foreach(Transform child in _spawnHolder)
        {
            Destroy(child.gameObject);
        }
    }

    private void TriggerSpawnSceneObjects(object sender, Scene scene)
    {
        //this.IsSpawning = true;

        this._spawnHolder = new GameObject(SPAWN_HOLDER_TRANSFORM_NAME).transform;


        if(scene.buildIndex >= GameSettings.PLAYABLE_SCENES_INDEX_RANGE.Item1 && scene.buildIndex <= GameSettings.PLAYABLE_SCENES_INDEX_RANGE.Item2 )
        {
            this.SpawnSceneObjects(this._labels);
        }
    }

    public void SpawnSceneObjects(List<AssetLabelReference> addressableLabels)
    {
        OverwriteSceneDataSave(addressableLabels);

        Addressables.LoadAssetsAsync<GameObject>(addressableLabels,
            InstantiateAsset,
            Addressables.MergeMode.Intersection,
             false
            ).Completed += (result) =>
            {
                Debug.Log($"[{result.Status}]: Spawn");

                SceneLoaderManager.Instance.ToggleLoadingScreen(false);
            };
    }

    private void InstantiateAsset(GameObject result)
    {
        if (result == null || _spawnHolder == null)
        {
            Debug.LogError("InstantiateAsset error. Missing holder or gameobjects");
        }
        
        Debug.Log($"Asset loaded: {result}");

        Instantiate(result, _spawnHolder);
    }


    private void OverwriteSceneDataSave(List<AssetLabelReference> addressableLabels)
    {
        Debug.LogWarning("Overwriting asset save");
        SceneSaveData currSceneData = SaveSystem.LoadSingle<SceneSaveData>(SaveSystem.SAVE_FILE_ID.SCENE_DATA);

        currSceneData.SceneAssetLabels = addressableLabels;
        SaveSystem.Save<SceneSaveData>(currSceneData, SaveSystem.SAVE_FILE_ID.SCENE_DATA);
    }
}