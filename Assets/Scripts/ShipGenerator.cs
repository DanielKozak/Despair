using ProceduralToolkit;
using UnityEngine;

public class ShipGenerator : MonoBehaviour
{
    [SerializeField] EventChannelShipSpawner spawnerChannel;

    [Space]
    [SerializeField] Transform shipParent;
    [SerializeField] GameObject shipPrefab;

    string[] _nameList;

    void Awake()
    {
        var file = Resources.Load<TextAsset>("Data/shipnames");
        _nameList = file.text.Split('\n');
    }

    public void OnEnable()
    {
        spawnerChannel.OnSpawnShipRequestEvent += OnSpawnShipRequest;
    }


    void OnDisable()
    {
        spawnerChannel.OnSpawnShipRequestEvent -= OnSpawnShipRequest;
    }
    private void OnSpawnShipRequest(ShipTypeSO typeSO)
    {
        Debug.Log($"ShipGenerator -> OnSpawnShipRequest");
    }

    void GenerateShip(ShipTypeSO typeSO)
    {
        GameObject newShip = Instantiate(shipPrefab, shipParent);
        GameObject newShipGraphics = Instantiate(typeSO.GraphicsSet.GetRandom(), newShip.transform);

        float angle = Random.Range(0f, 359f);
        float radius = Random.Range(8f, 15f);

        string shipName = GetRandomName();
        newShip.name = shipName;

        ShipController newShipController = newShip.GetComponent<ShipController>();
        newShipController.OrbitHeight = radius;
        newShipController.ShipName = shipName;

        newShip.transform.position = OrbitRenderer.GetPoint(radius, angle).normalized * 50f;
        newShipController.OrbitInsertionAngle = angle;

        newShipController.TypeData = typeSO;
        /*
                GameObject newOverlay = Instantiate(OverlayPrefab, OverlayParent.transform);
                newOverlay.GetComponent<ShipUIOverlay>().LinkedShip = newShipController;
                newOverlay.name = newShipController.ShipName + "_overlay";

                newShipController.SetOverlay(newOverlay);
                newShipController.timerCircle = newOverlay.GetComponent<ShipUIOverlay>().TimerMask.gameObject;
        */

    }

    string GetRandomName() => _nameList.GetRandom();

}

public enum ShipType
{
    Science, Engeneer, Military, Civilian
}
/*
 
        ShipController newShipController = newShip.GetComponent<ShipController>();
        newShipController.OrbitHeight = radius;
        newShipController.ShipName = newShip.name;
        newShip.transform.position = OrbitRenderer.GetPoint(radius, angle).normalized * 50f;
        newShipController.OrbitInsertionAngle = angle;

        newShipController.ShipType = type;

        switch (type)
        {
            case 0:
                newShip.GetComponentInChildren<SpriteRenderer>().color = ScienceShipColor;
                Debug.Log("Ship" + type + " " + newShip.GetComponentInChildren<SpriteRenderer>().color);

                newShipController.IntelMod = 1.5f;
                newShipController.FightMod = 0.5f;
                newShipController.ShipType = type;
                break;
            case 1:
                newShip.GetComponentInChildren<SpriteRenderer>().color = EngineerColor;
                Debug.Log("Ship" + type + " " + newShip.GetComponentInChildren<SpriteRenderer>().color);

                newShipController.RepairMod = 1.5f;
                newShipController.DespairMod = 0.5f;
                newShipController.ShipType = type;

                break;
            case 2:
                newShip.GetComponentInChildren<SpriteRenderer>().color = MilitaryColor;
                Debug.Log("Ship" + type + " " + newShip.GetComponentInChildren<SpriteRenderer>().color);

                newShipController.FightMod = 1.5f;
                newShipController.DespairMod = 0.5f;
                newShipController.ShipType = type;

                break;
            case 3:
                newShip.GetComponentInChildren<SpriteRenderer>().color = PassengerColor;
                Debug.Log("Ship" + type + " " + newShip.GetComponentInChildren<SpriteRenderer>().color);

                newShipController.FightMod = 0.5f;
                newShipController.DespairMod = 1.5f;
                newShipController.ShipType = type;

                break;
        }


        GameObject newOverlay = Instantiate(OverlayPrefab, OverlayParent.transform);
        newOverlay.GetComponent<ShipUIOverlay>().LinkedShip = newShipController;
        newOverlay.name = newShipController.ShipName + "_overlay";

        newShipController.SetOverlay(newOverlay);
        newShipController.timerCircle = newOverlay.GetComponent<ShipUIOverlay>().TimerMask.gameObject;


*/