using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public enum GameState
    {
        Menu, Playing, Paused, EndScreen
    }

    public GameState CurrentGameState;

    public GameObject ShipPrefab;
    public GameObject GameRoot;

    public GameObject OverlayPrefab;
    public GameObject OverlayParent;
    public GameObject LabelParent;


    public int HighScore = 0;
    public int DespairScore = 0;

    public GameObject Planet;

    public List<Skill> SkillList;

    void Start()
    {
        PopulateSkillList();
        HighScore = PlayerPrefs.GetInt("_highscore", 0);
        CurrentGameState = GameState.Menu;
    }

    public void GenerateShip()
    {
        float angle = (float)Random.Range(0, 359);
        float radius = (float)Random.Range(8, 15);

        GameObject newShip = Instantiate(ShipPrefab, GameRoot.transform);
        newShip.name = RandomName();
        ShipController newShipController = newShip.GetComponent<ShipController>();
        newShipController.OrbitHeight = radius;
        newShipController.ShipName = newShip.name;
        newShip.transform.position = OrbitRenderer.GetPoint(radius, angle).normalized * 50f;
        newShipController.OrbitInsertionAngle = angle;
        Debug.Log($"{angle} {radius}");

        GameObject newOverlay = Instantiate(OverlayPrefab, OverlayParent.transform);
        newOverlay.GetComponent<ShipUIOverlay>().LinkedShip = newShipController;

        newShipController.SetOverlay(newOverlay);



    }

    void PopulateSkillList()
    {
        SkillList = new List<Skill>();
        SkillList.Insert(0, new Skill_Boom());
    }


    string RandomName()
    {
        return "shipName";
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            GenerateShip();
    }


    public Color HPColor = new Color(0.6f, 0, 0, 1f);
    public Color IntelColor = new Color(0f, 0.5f, 0.06f, 1f);
    public Color DespairColor = new Color(0.6f, 0f, 0.9f, 1f);

}
