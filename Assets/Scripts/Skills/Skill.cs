using UnityEngine;
public class Skill
{
    public int ID;
    public string Name;
    public string Descr;

    public int cooldown;
    public int unlockLevel;

    public Texture2D icon;

    public string effectsDescr;

    public virtual void ApplyEffects(ShipController target) { }

}