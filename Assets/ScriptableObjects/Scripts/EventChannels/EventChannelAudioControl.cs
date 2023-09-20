using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelAudioControl", fileName = "EventChannelAudioControl")]
public class EventChannelAudioControl : ScriptableObject
{
    public UnityAction<AudioClip, Vector3> OnPlaySfxEvent;
    public UnityAction OnMusicPauseEvent;
    public UnityAction<AudioClip> OnMusicPlayEvent;


    public void RaiseOnPlaySfxEventEvent(AudioClip clip, Vector3 position)
    {
        DBG_LogManager.Instance.LogEvent(this, OnPlaySfxEvent, clip, position);
        OnPlaySfxEvent?.Invoke(clip, position);
    }
    public void RaiseOnMusicPauseEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, OnMusicPauseEvent);
        OnMusicPauseEvent?.Invoke();
    }
    public void RaiseOnMusicPlayEvent(AudioClip clip)
    {
        DBG_LogManager.Instance.LogEvent(this, OnMusicPlayEvent, clip);
        OnMusicPlayEvent?.Invoke(clip);
    }

}