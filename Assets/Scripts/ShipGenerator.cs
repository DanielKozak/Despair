using ProceduralToolkit;
using UnityEngine;

public class ShipGenerator : MonoBehaviour
{
    [SerializeField] EventChannelShipSpawner spawnerChannel;

    [Space]
    [SerializeField] Transform ShipParent;
    [SerializeField] GameObject prefab_Ship;

    [SerializeField] Transform OverlayParent;
    [SerializeField] UI_DBG_ShipOverlay prefab_Overlay;

    [SerializeField] Camera _Camera;

    string[] _nameList;

    void Awake()
    {
        var file = Resources.Load<TextAsset>("Data/shipnames");
        _nameList = file.text.Split('\n');
    }

    public void OnEnable()
    {
        spawnerChannel.OnSpawnShipRequestEvent += Callback_OnSpawnShipRequest;
    }


    void OnDisable()
    {
        spawnerChannel.OnSpawnShipRequestEvent -= Callback_OnSpawnShipRequest;
    }
    private void Callback_OnSpawnShipRequest(ShipTypeSO typeSO)
    {
        Debug.Log($"ShipGenerator -> OnSpawnShipRequest");
        GenerateShip(typeSO);
    }

    void GenerateShip(ShipTypeSO typeSO)
    {
        GameObject newShip = Instantiate(prefab_Ship, ShipParent);
        GameObject newShipGraphics = Instantiate(typeSO.GraphicsSet, newShip.transform);

        float angle = Random.Range(0f, 359f);
        float radius = Random.Range(8f, 15f);

        string shipName = GetRandomName();
        newShip.name = shipName;

        ShipController newShipController = newShip.GetComponent<ShipController>();
        newShipController.Init(typeSO, shipName, radius, angle);

        UI_DBG_ShipOverlay overlay = Instantiate(prefab_Overlay, OverlayParent);
        overlay.Init(newShipController, _Camera);
        newShipController.SetDBGOverlay(overlay);

    }

    string GetRandomName() => _nameList.GetRandom();

}

public enum ShipType
{
    Science, Engeneer, Military, Civilian
}
