using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelFactology", fileName = "EventChannelFactology")]
public class EventChannelFactology : ScriptableObject
{
    public UnityAction<string, bool> TriggerFactEvent;


    public void RaiseTriggerFactEvent(string factID, bool value)
    {
        DBG_LogManager.Instance.LogEvent(this, "TriggerFactEvent");

        TriggerFactEvent?.Invoke(factID, value);
    }
}

