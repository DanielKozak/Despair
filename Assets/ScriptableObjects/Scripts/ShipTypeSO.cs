using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Misc/ShipTypeSO")]
public class ShipTypeSO : ScriptableObject
{
    public ShipType typeID;
    public RandomPrefabChooserSO GraphicsSet;

    public float RepairMod;
    public float DespairMod;
    public float IntelMod;
    public float FightMod;
}
