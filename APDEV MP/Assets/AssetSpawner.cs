using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetSpawner : MonoBehaviour
{
    [SerializeField] private List<AssetLabelReference> _labels;
    [SerializeField] private List<GameObject> _spawnedObjects = new();
    public bool bDespawn;

    //FOR TESTING
    private void Start()
    {
        SpawnSceneObjects(this._labels);
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

    public void InstantiateAsset(GameObject result)
    {
        Debug.Log($"Asset loaded: {result}");
        this._spawnedObjects.Add(Instantiate(result));
    }
}