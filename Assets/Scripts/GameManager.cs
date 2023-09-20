using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EventChannelGameFlow _eventChannelGameFlow;


    [Space]

    public GameObject GameRoot;
    public GameObject MenuRoot;


    public GameObject OverlayPrefab;
    public GameObject OverlayParent;
    public GameObject LabelParent;



    public GameObject Planet;

    public Skill[] SkillList;
    public AbilitySO[] Abilities;




    public string LongestTortureName;
    public float LongestTortureTime;
    public int LongestTortureType;


    public bool isFromMenu = false;

    private void Awake()
    {
        _eventChannelGameFlow.OnGamePauseEvent += Callback_OnGamePauseEvent;
        _eventChannelGameFlow.OnGameUnpauseEvent += Callback_OnGameUnpauseEvent;
        _eventChannelGameFlow.OnShowMenuEvent += Callback_OnShowMenuEvent;
        _eventChannelGameFlow.OnNewGameStartEvent += Callback_OnNewGameStartEvent;
        _eventChannelGameFlow.OnTriggerGameOverEvent -= Callback_OnTriggerGameOverEvent;


        PopulateSkillList();
        PopulateAbilityList();
    }



    void Start()
    {
        _eventChannelGameFlow.RaiseOnShowMenuEvent();
    }

    private void Callback_OnGamePauseEvent()
    {
        Time.timeScale = 0f;
    }

    private void Callback_OnGameUnpauseEvent()
    {
        Time.timeScale = 1f;
    }

    private void Callback_OnShowMenuEvent()
    {
        GameRoot.SetActive(false);
        MenuRoot.SetActive(true);
    }

    private void Callback_OnNewGameStartEvent()
    {
        SetRandomPlanet();
        GameRoot.SetActive(true);
    }

    private void Callback_OnTriggerGameOverEvent()
    {
        StartCoroutine(GameOver());
    }
    void PopulateAbilityList()
    {
        Abilities = Resources.LoadAll<AbilitySO>("Abilities");
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

    private void OnApplicationQuit()
    {
        _eventChannelGameFlow.OnGamePauseEvent -= Callback_OnGamePauseEvent;
        _eventChannelGameFlow.OnGameUnpauseEvent -= Callback_OnGameUnpauseEvent;
        _eventChannelGameFlow.OnShowMenuEvent -= Callback_OnShowMenuEvent;
        _eventChannelGameFlow.OnNewGameStartEvent -= Callback_OnNewGameStartEvent;
        _eventChannelGameFlow.OnTriggerGameOverEvent -= Callback_OnTriggerGameOverEvent;
    }

}
