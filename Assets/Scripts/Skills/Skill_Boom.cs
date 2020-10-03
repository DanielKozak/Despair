using UnityEngine;
class Skill_Boom : Skill
{
    public Skill_Boom()
    {
        ID = 0;
        Name = "Micrometeor strike";
        Descr = "Galaxy's best buckshot.";
        cooldown = 5;
        unlockLevel = 0;
        //    icon = Resources.Load<Texture2D>("Icons/skill_boom_ico") as Texture2D;
        effectsDescr = "<color=red>-10HP</color><color=purple>+10 Despair</color>";
    }

    public override void ApplyEffects(ShipController target)
    {
        target.HP -= 10;
        target.Despair += 10;

        InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.HPColor, "-10", target.transform.position);
        //InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+10", target.transform.position, true);

    }
}