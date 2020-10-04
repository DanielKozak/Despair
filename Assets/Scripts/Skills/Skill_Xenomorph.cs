using UnityEngine;
class Skill_Xenomorph : Skill
{
    public int fightHP;
    public Skill_Xenomorph(string iconame)
    {
        ID = 3;
        Name = "Unleash a xenomorph";
        Descr = "The perfect organism. Its structural perfection is matched only by its hostility...its purity.";
        fightHP = 60;
        cooldown = 60;
        unlockLevel = 5;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "An alien creature tears through metal and flash alike, devastating the ship. <color=red>-60HP</color> and +30 Despair over time";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("alien");
        target.isBoarded = true;
        target.enemyHP += 60;
        target.Despair += 30;

        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.HPColor, "Boarded", target.transform.position);

    }
}