using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(menuName = "Abilities/Ability", fileName = "NewAbility")]
public class AbilitySO : ScriptableObject
{
    public int ID;
    public string NameKey;

    [TextArea(1, 20)]
    public string DescriptionKey;
    public int UnlockLevel;
    public bool isOneTimeUse;
    public bool isUseImmediate;
    public Sprite IconSmall;
    public Sprite IconLarge;
    public AudioClip audioClip;
    public AbilityBehaviourSO Effect;
    public List<UpgradeableParam> uParamList;

    public void Execute(Vector3 worldPos)
    {
        Effect?.ApplyEffects(this, worldPos);
    }


    public override bool Equals(object other)
    {
        if (((AbilitySO)other).ID == ID) return true;
        return false;
    }


    public UpgradeableParam GetParam(string name)
    {
        if (uParamList.Count == 0) throw new Exception("Forgot to add params");
        return uParamList.Where(p => p.Name.Equals(name)).First();
    }
}

[Serializable]
public class UpgradeableParam
{
    public string Name;
    [TextArea(1, 20)]
    public string Description;
    public Sprite Icon;
    public int currentLevel = 1;
    public int maxLevel = 1;
    public float value = 0f;

    public float GetValue() => value;
}
