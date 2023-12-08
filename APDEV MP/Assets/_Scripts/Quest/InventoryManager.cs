using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            SceneManager.sceneLoaded += LoadInventory;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LoadInventory;
    }


    public bool HasItem(string itemName)
    {
        return this._itemNames.Contains(itemName);
    }

    public void AddItem(string itemName)
    {
        this._itemNames.Add(itemName);
        this.SaveInventory();
    }

    public void EraseInvetory()
    {
        this._itemNames.Clear();
        this.SaveInventory();
    }

    private void SaveInventory()
    {
        SaveSystem.Save<string>(this._itemNames, SaveSystem.SAVE_FILE_ID.INVENTORY_DATA);
    }


    private void LoadInventory(Scene scene, LoadSceneMode mode)
    {
        //CANCEL LOADING WHEN THE GAME IS OVER
        if(scene.buildIndex == 0)
        {
            return;
        }

        List<string> lastSave = SaveSystem.LoadList<string>(SaveSystem.SAVE_FILE_ID.INVENTORY_DATA);
        if(lastSave != null)
        {
            this._itemNames = lastSave;
        }
    }
}