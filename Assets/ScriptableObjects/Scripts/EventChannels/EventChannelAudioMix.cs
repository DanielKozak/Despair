using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelAudioMix", fileName = "EventChannelAudioMix")]
public class EventChannelAudioMix : ScriptableObject
{
    public UnityAction<float> OnSFXVolumeChanged;
    public UnityAction<float> OnMusicVolumeChanged;
    public UnityAction OnToggleMute;


    public void RaiseOnToggleMuteEvent()
    {
        OnToggleMute?.Invoke();
    }
    public void RaiseOnSFXVolumeChangedEvent(float newVolume)
    {
        OnSFXVolumeChanged?.Invoke(newVolume);
    }
    public void RaiseOnMusicVolumeChangedEvent(float newVolume)
    {
        OnMusicVolumeChanged?.Invoke(newVolume);
    }

}