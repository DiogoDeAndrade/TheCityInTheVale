using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SetAllMaterials : MonoBehaviour
{
    public Material material;

    [Button("Set materials")]
    void SetMaterials()
    {
        var allMR = GetComponentsInChildren<MeshRenderer>();
        foreach (var mr in allMR)
        {
            mr.sharedMaterial = material;
        }
    }
}
