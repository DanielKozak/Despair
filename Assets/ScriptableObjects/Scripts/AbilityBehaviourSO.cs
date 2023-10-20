using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBehaviourSO : ScriptableObject
{
    public abstract void ApplyEffects(AbilitySO ability, Vector3 worldPos);
}
