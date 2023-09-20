using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill", fileName = "NewSkill")]
public class AbilitySO : ScriptableObject
{
    public int ID;
    public string NameKey;

    [TextArea(1, 20)]
    public string DescriptionKey;
    public float Cooldown;
    public int UnlockLevel;
    public Sprite IconSmall;
    public Sprite IconLarge;
    public AudioClip audioClip;
    public AbilityEffectSO Effect;
    public AbilitySO ReplacesAbility;


}
