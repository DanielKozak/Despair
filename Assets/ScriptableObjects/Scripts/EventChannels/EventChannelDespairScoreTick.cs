using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelDespairScoreTick", fileName = "EventChannelDespairScoreTick")]
public class EventChannelDespairScoreTick : ScriptableObject
{
    public UnityAction<int> OnDespairTickEvent;


    public void RaiseOnDespairTickEventEvent(int amount)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnDespairTickEvent", amount);

        OnDespairTickEvent?.Invoke(amount);
    }

}