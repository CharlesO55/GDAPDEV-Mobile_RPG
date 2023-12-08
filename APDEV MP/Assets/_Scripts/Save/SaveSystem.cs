using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


/*
 * TEMPLATE FROM Unity Tutorial on Saving/Loading JSONs https://youtu.be/KZft1p8t2lQ?si=XyqY6l-ksHb53wgx
 */
public static class SaveSystem
{
    private static readonly string SAVE_FOLDER = /*Application.dataPath */ Application.persistentDataPath + "/Saves/";

    public enum SAVE_FILE_ID
    {
        PARTY_DATA,
        QUESTS_DATA,
        SCENE_DATA,
        INVENTORY_DATA
    }


    private static Dictionary<SAVE_FILE_ID, string> fileNamesDictionary;

    //MAKE SURE TO INITIALIZE FIRST W/ GAME MANAGER
    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        fileNamesDictionary = new Dictionary<SAVE_FILE_ID, string>()
        {
            { SAVE_FILE_ID.PARTY_DATA, "PartyData.json" },
            { SAVE_FILE_ID.QUESTS_DATA, "QuestData.json" },
            { SAVE_FILE_ID.SCENE_DATA, "SceneData.json" },
            { SAVE_FILE_ID.INVENTORY_DATA, "InventoryData.json"}
        };

    }



    //FOR SAVING LISTS
    public static void Save<T>(List<T> itemToSave, SAVE_FILE_ID jsonFileID)
    {
        string fileLoc = SAVE_FOLDER + fileNamesDictionary[jsonFileID];
        Debug.Log("Saving " + itemToSave + " to " + fileLoc);

        FileStream fileStream = new FileStream(fileLoc, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            //USE JSONHELPER
            string dataAsJSON = JsonHelper.ToJson(itemToSave.ToArray());

            writer.Write(dataAsJSON);
        }

        fileStream.Close();
    }

    //FOR SAVING SINGLE ITEMS
    public static void Save<T>(T itemToSave, SAVE_FILE_ID jsonFileID)
    {
        string fileLoc = SAVE_FOLDER + fileNamesDictionary[jsonFileID];
        Debug.Log("Saving " + itemToSave + " to " + fileLoc);

        FileStream fileStream = new FileStream(fileLoc, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            //USE STANDARD JSON UTILITY
            string dataAsJSON = JsonUtility.ToJson(itemToSave);
            
            writer.Write(dataAsJSON);
        }

        fileStream.Close();
    }

    public static List<T> LoadList<T>(SAVE_FILE_ID jsonFileID)
    {
        string jsonContent = ReadJSON(jsonFileID);
        Debug.Log("Extracted " + jsonContent);


        if(jsonContent == null || jsonContent == "{}") {
            Debug.LogError("JSON is empty");
            return new List<T>();
        }

        T[] result = JsonHelper.FromJson<T>(jsonContent);
        return result.ToList();
    }

    public static T LoadSingle<T>(SAVE_FILE_ID jsonFileID)
    {
        string jsonContent = ReadJSON(jsonFileID);
        Debug.Log("Extracted " + jsonContent);


        if (jsonContent == null || jsonContent == "{}")
        {
            Debug.LogError("JSON is empty");
            return default (T);
        }

        T result = JsonUtility.FromJson<T>(jsonContent);
        return result;
    }

    private static string ReadJSON(SAVE_FILE_ID jsonFileID)
    {
        string fileLoc = SAVE_FOLDER + fileNamesDictionary[jsonFileID];
        Debug.Log("Reading " + fileLoc);

        if (!File.Exists(fileLoc))
        {
            Debug.LogWarning("Does not exist: " + jsonFileID);
            return null;
        }

        using (StreamReader reader = new StreamReader(fileLoc))
        {
            return reader.ReadToEnd();
        }
    }

    
}



/*  LMAO, my brain is too smooth right now.
 *  Wrapper that basically allows us to store lists instead of just single items in JSON
 *  Stackoverflow copy-paste magic : https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
 */

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}