using UnityEngine;
class Skill_Divine : Skill
{
    public Skill_Divine(string iconame)
    {
        ID = 7;
        Name = "Divine Interference";
        Descr = "Fracked up systems started magically working ?";
        cooldown = 45;
        unlockLevel = 4;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "Repairs <color=red>+30HP</color>on a ship";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("mechanics");

        target.HP += 30;

        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.HPColor, "+30", target.transform.position);
        //InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+10", target.transform.position, true);

    }
}