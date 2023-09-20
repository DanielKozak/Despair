using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Effects/AddDespair", fileName = "EffectAddDespair")]
public class EffectAddDespair : AbilityEffectSO
{
    public int value;
    public override void ApplyEffects(ShipController target)
    {
        target.Despair += value;
        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, $"+{value}", target.transform.position);

    }
}
