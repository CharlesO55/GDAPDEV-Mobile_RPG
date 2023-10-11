using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxTool : MonoBehaviour
{
    BoxCollider _boxCollider;

    [Tooltip("Is the spawn box visible")]
    [SerializeField] private bool bVisibleOnStart = true;

    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();

        this.GetComponent<MeshRenderer>().enabled = bVisibleOnStart;
    }

    public Vector3 getRandomSpawnPos()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(_boxCollider.bounds.min.x, _boxCollider.bounds.max.x),
            Random.Range(_boxCollider.bounds.min.y, _boxCollider.bounds.max.y),
            Random.Range(_boxCollider.bounds.min.z, _boxCollider.bounds.max.z)
            );
        return spawnPos;
    }
}
