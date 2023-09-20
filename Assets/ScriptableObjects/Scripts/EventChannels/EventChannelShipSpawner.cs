using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelShipSpawner", fileName = "EventChannelShipSpawner")]
public class EventChannelShipSpawner : ScriptableObject
{
    public UnityAction<ShipTypeSO> OnSpawnShipRequestEvent;
    public UnityAction<Transform> OnSpawnOverlayRequestEvent;
    public void RaiseOnSpawnShipRequestEvent(ShipTypeSO typeSO)
    {
        OnSpawnShipRequestEvent?.Invoke(typeSO);
    }
    public void RaiseOnSpawnOverlayRequestEvent(Transform shipTransform)
    {
        OnSpawnOverlayRequestEvent?.Invoke(shipTransform);
    }

}