using UnityEngine;
class Skill_Meteor : Skill
{
    public Skill_Meteor(string iconame)
    {
        ID = 0;
        Name = "Micrometeor strike";
        Descr = "Galaxy's best buckshot.";
        cooldown = 10;
        unlockLevel = 1;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = $"Damages <color=red>-10HP</color> and adds <color={GameManager.Instance.DespairColor}>+10 Despair</color> to ships crew";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("meteor");

        target.HP -= 10;
        target.Despair += 10;

        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.HPColor, "-10", target.transform.position);
        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+10", target.transform.position, true);


    }
}