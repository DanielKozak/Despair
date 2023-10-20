using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelAudioMix", fileName = "EventChannelAudioMix")]
public class EventChannelAudioMix : ScriptableObject
{
    public UnityAction<float> OnSFXVolumeChangedEvent;
    public UnityAction<float> OnMusicVolumeChangedEvent;
    public UnityAction OnToggleMuteEvent;


    public void RaiseOnToggleMuteEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, "OnToggleMuteEvent");
        OnToggleMuteEvent?.Invoke();
    }
    public void RaiseOnSFXVolumeChangedEvent(float newVolume)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnSFXVolumeChangedEvent", newVolume);
        OnSFXVolumeChangedEvent?.Invoke(newVolume);
    }
    public void RaiseOnMusicVolumeChangedEvent(float newVolume)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnMusicVolumeChangedEvent", newVolume);
        OnMusicVolumeChangedEvent?.Invoke(newVolume);
    }

}