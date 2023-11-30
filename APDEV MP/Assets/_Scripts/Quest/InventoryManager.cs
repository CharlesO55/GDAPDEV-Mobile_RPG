using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [SerializeField] private List<string> _itemNames = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public bool HasItem(string itemName)
    {
        return this._itemNames.Contains(itemName);
    }

    public void AddItem(string itemName)
    {
        this._itemNames.Add(itemName);
    }
}
