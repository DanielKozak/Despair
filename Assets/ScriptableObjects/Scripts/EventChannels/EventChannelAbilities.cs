using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelAbilities", fileName = "EventChannelAbilities")]
public class EventChannelAbilities : ScriptableObject
{
    public UnityAction<AbilitySO> OnAbilitySelectedEvent;
    public UnityAction OnAbilityDeselectedEvent;
    public UnityAction<Vector3, AbilitySO> OnAbilityUsedEvent;

    public void RaiseOnSkillSelectedEvent(AbilitySO ability)
    {
        DBG_LogManager.Instance.LogEvent(this, OnAbilitySelectedEvent, ability);
        OnAbilitySelectedEvent?.Invoke(ability);
    }
    public void RaiseOnSkillDeselectedEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, OnAbilityDeselectedEvent);
        OnAbilityDeselectedEvent?.Invoke();
    }
    public void RaiseOnAbilityUsedEvent(Vector3 worldPos, AbilitySO ability)
    {
        DBG_LogManager.Instance.LogEvent(this, OnAbilityUsedEvent, worldPos, ability);
        OnAbilityUsedEvent?.Invoke(worldPos, ability);
    }

}