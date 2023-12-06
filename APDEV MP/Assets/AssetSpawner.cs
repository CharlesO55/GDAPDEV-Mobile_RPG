using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class AssetSpawner : MonoBehaviour
{
    public static AssetSpawner Instance;

    [SerializeField] private List<AssetLabelReference> _labels;
    [SerializeField] private List<GameObject> _spawnedObjects = new();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += TriggerSpawnSceneObjects;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= TriggerSpawnSceneObjects;
    }

    public void MarkNextSceneAssets(List<AssetLabelReference> nextSceneLabels)
    {
        this._labels = nextSceneLabels;
    }

    public void DespawnObjects()
    {
        foreach(GameObject obj in _spawnedObjects)
        {
            Destroy(obj);
        }

        _spawnedObjects.Clear();
    }

    private void TriggerSpawnSceneObjects(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex >= GameSettings.PLAYABLE_SCENES_INDEX_RANGE.Item1 && scene.buildIndex <= GameSettings.PLAYABLE_SCENES_INDEX_RANGE.Item2 )
        {
            this.SpawnSceneObjects(this._labels);
        }
    }

    public void SpawnSceneObjects(List<AssetLabelReference> addressableLabels)
    {
        Addressables.LoadAssetsAsync<GameObject>(addressableLabels,
            InstantiateAsset,
            Addressables.MergeMode.Intersection,
             false
            ).Completed += (result) =>
            {
                Debug.Log($"[{result.Status}]: Spawn");
            };
    }

    private void InstantiateAsset(GameObject result)
    {
        Debug.Log($"Asset loaded: {result}");
        this._spawnedObjects.Add(Instantiate(result));
    }
}