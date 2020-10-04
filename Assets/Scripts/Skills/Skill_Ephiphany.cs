using UnityEngine;
class Skill_Ephiphany : Skill
{
    public Skill_Ephiphany(string iconame)
    {
        ID = 8;
        Name = "Ephiphany";
        Descr = "This place is beautiful.Mesmerising.";
        cooldown = 60;
        unlockLevel = 8;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "Crew Despair reduced by 30";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("heal");

        target.Despair -= 30;

        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "-30", target.transform.position);
        //InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+10", target.transform.position, true);

    }
}