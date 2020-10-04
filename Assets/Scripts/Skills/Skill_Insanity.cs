using UnityEngine;
class Skill_Insanity : Skill
{
    public int fightHP;
    public Skill_Insanity(string iconame)
    {
        ID = 1;
        Name = "Insanity";
        Descr = "Some of the crewmates lost their minds!";
        fightHP = 20;
        cooldown = 30;
        unlockLevel = 2;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "Ship's crew fight the Insane, losing hp and gaining Despair";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("madness");

        target.isBoarded = true;
        target.enemyHP = 20;

        InterfaceManager.Instance.ShowAnimatedLabel(Color.green, "INSANITY", target.transform.position);
        //InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+10", target.transform.position, true);

    }
}