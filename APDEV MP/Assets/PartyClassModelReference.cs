using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitclassModelLibrary : MonoBehaviour
{
    public static UnitclassModelLibrary Instance;

    public List<EnumUnitClass> UnitClasses = new();
    public List<GameObject> UnitModels = new();



    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public GameObject GetUnitModel(EnumUnitClass unitClass)
    {
        for(int i = 0; i < UnitClasses.Count; i++)
        {
            if (UnitClasses[i] == unitClass)
            {
                return UnitModels[i];
            }
        }

        Debug.LogError($"ERROR Failed to find prefab model for {unitClass}");
        return null;
    }
}