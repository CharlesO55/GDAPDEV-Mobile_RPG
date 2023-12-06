using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "Unit Preset", menuName = "ScriptableObjects/Unit Preset")]
public class SO_UnitInfo : ScriptableObject
{
    [SerializeField] List<CharacterData> _unitPresetData = new();
    public List<CharacterData> UnitPresetDataList
    {
        get { return _unitPresetData; }
    }

    public CharacterData GetUnitPresetData(EnumUnitClass unitClass)
    {
        return this._unitPresetData.Find((preset) => preset.CharacterClass == unitClass);
    }
}