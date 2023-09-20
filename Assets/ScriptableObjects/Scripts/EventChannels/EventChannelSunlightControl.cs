using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelSunlightControl", fileName = "EventChannelSunlightControl")]
public class EventChannelSunlightControl : ScriptableObject
{
    public UnityAction<float> OnSunlightRotationChangedEvent;


    public void RaiseOnSunlightRotationChangedEvent(float newRotation)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnSunlightRotationChangedEvent");

        OnSunlightRotationChangedEvent?.Invoke(newRotation);
    }

}