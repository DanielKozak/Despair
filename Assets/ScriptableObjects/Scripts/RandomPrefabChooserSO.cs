using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;
[CreateAssetMenu(menuName = "Misc/RandomPrefabChooserScriptableObject")]
public class RandomPrefabChooserSO : ScriptableObject
{
    [SerializeField] List<GameObject> Variants = new List<GameObject>();

    public GameObject GetRandom() => Variants.GetRandom();
}
