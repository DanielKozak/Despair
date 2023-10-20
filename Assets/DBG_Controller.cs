using System.Collections;
using System.Collections.Generic;
using ProceduralToolkit;
using UnityEngine;

public class DBG_Controller : MonoBehaviour
{

    EventChannelDespairScoreTick _eventChannelDespairScoreTick;
    EventChannelLevelUp _eventChannelLevelUp;
    EventChannelShipSpawner _eventChannelShipSpawner;
    [SerializeField] KeyCode Add10DespairKeyCode = KeyCode.F2;
    [SerializeField] KeyCode Add100DespairKeyCode = KeyCode.F3;
    [SerializeField] KeyCode SpawnShip0KeyCode = KeyCode.F9;

    ShipTypeSO[] ShipPrefabs;

    void Start()
    {
        _eventChannelDespairScoreTick = RuntimeEventChannelContainer.Instance.EventChannelDespairScoreTickInstance;
        _eventChannelLevelUp = RuntimeEventChannelContainer.Instance.EventChannelLevelUpInstance;
        _eventChannelShipSpawner = RuntimeEventChannelContainer.Instance.EventChannelShipSpawnerInstance;

        ShipPrefabs = Resources.LoadAll<ShipTypeSO>("ShipTypes");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(Add10DespairKeyCode))
        {
            _eventChannelDespairScoreTick.RaiseOnDespairTickEvent(10);
        }
        if (Input.GetKeyUp(Add100DespairKeyCode))
        {
            _eventChannelDespairScoreTick.RaiseOnDespairTickEvent(100);
        }
        if (Input.GetKeyUp(SpawnShip0KeyCode))
        {
            _eventChannelShipSpawner.RaiseOnSpawnShipRequestEvent(ShipPrefabs.GetRandom());
        }
    }
}
