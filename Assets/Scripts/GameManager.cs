using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    private GameState _currentState = GameState.Menu;
    public GameState CurrentGameState
    {
        get { return _currentState; }
        set
        {
            switch (value)
            {
                case GameState.Menu:
                    //MENUROOT
                    break;
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;
                case GameState.EndScreen:
                    //Endscreen
                    //HighScore
                    break;

            }
        }
    }

    public GameObject ShipPrefab;
    public GameObject GameRoot;

    public GameObject OverlayPrefab;
    public GameObject OverlayParent;
    public GameObject LabelParent;


    public int HighScore = 0;
    public int DespairScore = 0;

    public GameObject Planet;

    public Skill[] SkillList;

    public string[] NameList;

    public GameObject MenuRoot;


    public string LongestTortureName;
    public float LongestTortureTime;
    public int LongestTortureType;


    public bool isFromMenu = false;


    void Start()
    {
        Debug.Log("BLYAD");
        PopulateSkillList();

        //if(PlayerPrefs.HasKey("_highscore"))
        HighScore = PlayerPrefs.GetInt("_highscore", 0);
        LongestTortureName = PlayerPrefs.GetString("_torturedName", "");
        LongestTortureTime = PlayerPrefs.GetFloat("_torturedTime", 0);
        LongestTortureType = PlayerPrefs.GetInt("_torturedtype", 0);
        CurrentGameState = GameState.Menu;
        MenuRoot.SetActive(true);
        //StartGame();
    }

    public void GenerateShip()
    {
        ShipCount++;
        int type = Random.Range(0, 4);
        float angle = (float)Random.Range(0, 359);
        float radius = (float)Random.Range(8, 15);
        GameObject newShip = Instantiate(ShipPrefab, GameRoot.transform);
        newShip.name = RandomName();
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

    }

    void PopulateSkillList()
    {
        SkillList = new Skill[9];
        Skill s = new Skill_Depression("bg_depression");
        SkillList[s.ID] = s;
        s = new Skill_Divine("bg_intervention");
        SkillList[s.ID] = s;
        s = new Skill_Ephiphany("bg_heal");
        SkillList[s.ID] = s;
        s = new Skill_Insanity("bg_insane");
        SkillList[s.ID] = s;
        s = new Skill_Interference("bg_interference");
        SkillList[s.ID] = s;
        s = new Skill_Meteor("bg_meteor");
        SkillList[s.ID] = s;
        s = new Skill_Physics("bg_physics");
        SkillList[s.ID] = s;
        s = new Skill_Sos("bg_sos");
        SkillList[s.ID] = s;
        s = new Skill_Xenomorph("bg_xeno");
        SkillList[s.ID] = s;
    }

    public bool inTutorial = false;
    public void StartGame()
    {

        MenuRoot.SetActive(false);
        GameRoot.SetActive(true);
        StartCoroutine(StartGameRoutine());
        SetRandomPlanet();
        var file = Resources.Load<TextAsset>("shipnames");
        NameList = file.text.Split('\n');
        InterfaceManager.Instance.HideUnlockPanel();
        CurrentGameState = GameState.Playing;
    }

    IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(3f);
        GenerateShip();

        yield return new WaitForSeconds(10f);
        GenerateShip();
    }


    string RandomName()
    {
        int index = Random.Range(0, NameList.Length);
        return NameList[index];
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
            GenerateShip();
        if (Input.GetKeyUp(KeyCode.F2))
            ProgressManager.Instance.Score += 100;

    }

    public int ShipCount = 0;

    public Color HPColor = new Color(0.6f, 0, 0, 1f);
    public Color IntelColor = new Color(0f, 0.5f, 0.06f, 1f);
    public Color DespairColor = new Color(0.6f, 0f, 0.9f, 1f);

    public List<Sprite> planetList = new List<Sprite>();

    void SetRandomPlanet()
    {
        int x = Random.Range(0, planetList.Count);
        Planet.GetComponent<SpriteRenderer>().sprite = planetList[x];
    }

    public void SummonShip()
    {
        StartCoroutine(SummonRoutine());
    }

    IEnumerator SummonRoutine()
    {
        float x = Random.Range(0f, 10f);
        yield return new WaitForSeconds(x);
        GenerateShip();
    }

    public int InformationGathered = 0;

    public void UpdateInformationGathered(int delta)
    {
        InformationGathered += delta;
        InterfaceManager.Instance.InformationLabel.DOText($"Information Gathered {InformationGathered}/500Pb", 1f, true, ScrambleMode.Lowercase);

        if (InformationGathered >= 500)
        {
            StartCoroutine(GameOver());
        }
    }

    public void ExitToMenu()
    {
        LogDump.Dump();
        /*var ships = GameObject.FindObjectsOfType<ShipController>();
        foreach (var item in ships)
        {
            item.CompareShipData();
        }*/
        isFromMenu = true;
        CurrentGameState = GameState.Menu;
        if (ProgressManager.Instance.Score >= HighScore)
        {
            PlayerPrefs.SetInt("_highscore", ProgressManager.Instance.Score);
        }
        GameRoot.SetActive(false);
        MenuRoot.SetActive(true);
    }


    Color ScienceShipColor = new Color(1f, 1f, 1f, 1f);
    Color EngineerColor = new Color(1f, 0.5f, 0f, 1f);
    Color MilitaryColor = new Color(1f, 0f, 0f, 1f);
    Color PassengerColor = new Color(0f, 0.3f, 1f, 1f);


    public static event System.EventHandler OnGameOver;
    public IEnumerator GameOver()
    {
        OnGameOver?.Invoke(this, null);
        yield return null;

    }

}
