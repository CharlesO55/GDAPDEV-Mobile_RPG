using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName ="Default Audio Database", menuName ="ScriptableObjects/SFX Database")]
public class SFXDatabase : ScriptableObject
{
    public List<EnumSFX> SFX_ID = new();
    public List<AssetReference> ClipReference = new();

    public AssetReference GetClipReference(EnumSFX sfxID)
    {
        for(int i = 0; i < SFX_ID.Count; i++)
        {
            if (SFX_ID[i] == sfxID)
            {
                return ClipReference[i];
            }
        }
        Debug.LogError($"Failed to find {sfxID}");
        return null;
    }
}