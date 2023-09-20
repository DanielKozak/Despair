using UnityEngine;
class Skill_Sos : Skill
{
    public Skill_Sos(string iconame)
    {
        ID = 6;
        Name = "S.O.S";
        Descr = "Pirate bait? That far outside the shipping lanes?...";
        cooldown = 30;
        unlockLevel = 6;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "Sends out a SOS signal to summon unsuspecting ships";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("sos");

        // GameManager.Instance.SummonShip();
    }
}