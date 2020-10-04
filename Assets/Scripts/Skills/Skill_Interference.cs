using UnityEngine;
class Skill_Interference : Skill
{
    public int duration;
    public Skill_Interference(string iconame)
    {
        ID = 5;
        Name = "Interference";
        Descr = "A thick cloud of exotic particles blinds the sensors for a while.";
        duration = 180;
        cooldown = 60;
        unlockLevel = 7;
        icon = Resources.Load<Sprite>($"Skills/{iconame}") as Sprite;
        effectsDescr = "Target's scaning speed reduced by half for 2 minutes";
    }

    public override void ApplyEffects(ShipController target)
    {
        AudioManager.PlaySound("interference");

        target.StartCoroutine(target.InterferenceRoutine(duration));
        InterfaceManager.Instance.ShowAnimatedLabel(Color.green, "Interference", target.transform.position);

    }
}