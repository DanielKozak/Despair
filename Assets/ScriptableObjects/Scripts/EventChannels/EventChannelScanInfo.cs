using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelScanInfo", fileName = "EventChannelScanInfo")]
public class EventChannelScanInfo : ScriptableObject
{
    public UnityAction<int> OnScanInfoLevelChangedEvent;


    public void RaiseOnScanInfoLevelChangedEvent(int newScanInfoAmount)
    {
        DBG_LogManager.Instance.LogEvent(this, "OnScanInfoLevelChangedEvent", newScanInfoAmount);
        OnScanInfoLevelChangedEvent?.Invoke(newScanInfoAmount);
    }

}