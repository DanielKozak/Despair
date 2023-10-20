using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Effects/EffectPrefab", fileName = "EffectPrefab")]
public class EffectPrefab : AbilityBehaviourSO
{

    public override void ApplyEffects(AbilitySO ability, Vector3 worldPos)
    {
        Debug.Log($"Use {ability.NameKey} @ {worldPos}");
        InterfaceManager.Instance.ShowAnimatedLabel(Color.green, $"Use {ability.NameKey} @ {worldPos}", worldPos);
    }
}
