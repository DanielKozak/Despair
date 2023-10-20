using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelLevelUp", fileName = "EventChannelLevelUp")]
public class EventChannelLevelUp : ScriptableObject
{
    public UnityAction<int> OnLevelUpEvent;
    public UnityAction<AbilitySO> OnAbilityChosenLevelUpEvent;
    public UnityAction<AbilitySO, UpgradeableParam> OnAbilityUpgradedLevelUpEvent;


    public void RaiseOnLevelUpEvent(int newLevel)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnLevelUpEvent", newLevel);
        OnLevelUpEvent?.Invoke(newLevel);
    }
    public void RaiseOnAbilityChosenLevelUpEvent(AbilitySO ability)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnAbilityChosenLevelUpEvent", ability);
        OnAbilityChosenLevelUpEvent?.Invoke(ability);
    }
    public void RaiseOnAbilityUpgradedLevelUpEvent(AbilitySO ability, UpgradeableParam upgradeable)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnAbilityChosenLevelUpEvent", ability, upgradeable);
        OnAbilityUpgradedLevelUpEvent?.Invoke(ability, upgradeable);
    }

}