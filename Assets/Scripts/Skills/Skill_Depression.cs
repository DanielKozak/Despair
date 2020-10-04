using UnityEngine;
class Skill_Depression : Skill
{
    public Skill_Depression(string iconame)
    {
        ID = 4;
        Name = "Depression";
        Descr = "Concern should drive us into action and not into a depression. No man is free who cannot control himself.";
        cooldown = 45;
        unlockLevel = 5;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "Adds <color=purple>+30 Despair</color> to ships crew";
    }

    public override void ApplyEffects(ShipController target)
    {
        target.Despair += 30;

        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+30", target.transform.position);
        //InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+10", target.transform.position, true);

    }
}