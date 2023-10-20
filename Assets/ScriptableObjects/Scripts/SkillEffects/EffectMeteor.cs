using UnityEngine;
using DG.Tweening;


[CreateAssetMenu(menuName = "Skills/Effects/EffectMeteor", fileName = "EffectMeteor")]
public class EffectMeteor : AbilityBehaviourSO
{
    [SerializeField] AsteroidEffect prefab_Meteor;
    public override void ApplyEffects(AbilitySO ability, Vector3 worldPos)
    {
        // float damage = ability.GetParam("damage").value;
        worldPos.z = 0f;
        Vector2 spawnPos = Random.insideUnitCircle * 100f;

        AsteroidEffect meteor = Instantiate(prefab_Meteor, spawnPos, Quaternion.identity);
        meteor.Init(worldPos, ability.GetParam("damage").value);
        meteor = null;
        // DOVirtual.DelayedCall(0.05f, () => );
        Debug.Log($"Use {ability.NameKey} @ {worldPos}");
        // InterfaceManager.Instance.ShowAnimatedLabel(Color.green, $"Use {ability.NameKey} @ {worldPos}", worldPos);


    }
}
