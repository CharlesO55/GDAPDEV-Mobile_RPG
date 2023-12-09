using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Highlighter
{
    private static float EMISSIVE_INTENSITY = 0.5f;
    public static void HighlightObject(GameObject objToHighlight, Color highlightColor)
    {
        //EXTRACT THE MATERIALS
        List<Material> mats = new();

        //GET THIS THIS MATERIALS OR...
        if (objToHighlight.TryGetComponent<Renderer>(out Renderer thisRenderer))
        {
            mats.AddRange(thisRenderer.materials);
        }
        //GET THE CHILD'S MATERIALS
        else if (objToHighlight.GetComponentsInChildren<Renderer>() != null)
        {
            foreach (Renderer childRenderer in objToHighlight.GetComponentsInChildren<Renderer>())
            {
                mats.AddRange(childRenderer.materials);
            }
        }
        else
        {
            Debug.LogError($"{objToHighlight.name} has no Renderer to extract Materials from");
        }

        //HIGHLIGHT THE OBJECT

        if (highlightColor != Color.black)
        {
            foreach (Material mat in mats)
            {
                mat.EnableKeyword("_EMISSION");
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                mat.SetColor("_EmissionColor", highlightColor * EMISSIVE_INTENSITY);

                
            }
        }
        else
        {
            foreach (Material mat in mats)
            {
                mat.SetColor("_EmissionColor", highlightColor);
                mat.DisableKeyword("_EMISSION");
            }
        }
        
    }
}
