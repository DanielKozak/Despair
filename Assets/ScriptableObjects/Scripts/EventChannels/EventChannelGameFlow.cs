using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelGameFlow", fileName = "EventChannelGameFlow")]
public class EventChannelGameFlow : ScriptableObject
{
    public UnityAction OnNewGameStartEvent;
    public UnityAction OnGamePauseEvent;
    public UnityAction OnGameUnpauseEvent;
    public UnityAction OnShowMenuEvent;
    public UnityAction OnTriggerGameOverEvent;


    public void RaiseOnNewGameStartEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, "OnNewGameStartEvent");
        OnNewGameStartEvent?.Invoke();
    }
    public void RaiseOnGamePauseEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, "OnGamePauseEvent");
        OnGamePauseEvent?.Invoke();
    }
    public void RaiseOnGameUnpauseEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, "OnGameUnpauseEvent");
        OnGameUnpauseEvent?.Invoke();
    }
    public void RaiseOnShowMenuEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, "OnShowMenuEvent");
        OnShowMenuEvent?.Invoke();
    }
    public void RaiseOnTriggerGameOverEvent()
    {
        DBG_LogManager.Instance.LogEvent(this, "OnTriggerGameOverEvent");
        OnTriggerGameOverEvent?.Invoke();
    }

}