using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffectSO : ScriptableObject
{
    [TextArea(1, 20)]
    public string EffectsDescriptionKey;

    public abstract void ApplyEffects(ShipController target);
}
