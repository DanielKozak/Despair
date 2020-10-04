using UnityEngine;
class Skill_Physics : Skill
{
    public int duration;
    public Skill_Physics(string iconame)
    {
        ID = 2;
        Name = "Physics manipulation";
        Descr = "Old blue goo is at it again. We cannot repair anything.";
        duration = 30;
        cooldown = 45;
        unlockLevel = 3;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "Ship's crew cannot repair anything for 30 seconds.";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("physics");

        target.StartCoroutine(target.PhysicsRoutine(duration));
        InterfaceManager.Instance.ShowAnimatedLabel(Color.white, "Frozen", target.transform.position);
        //InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, "+10", target.transform.position, true);

    }
}