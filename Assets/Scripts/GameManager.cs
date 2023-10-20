using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EventChannelGameFlow _eventChannelGameFlow;
    [SerializeField] private EventChannelLevelUp _eventChannelLevelUp;


    [Space]

    public GameObject GameRoot;
    public GameObject MenuRoot;
    public GameObject Planet;

    public AbilitySO[] Abilities;
    public List<AbilitySO> UnlockedAbilities;


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
        _eventChannelGameFlow.OnTriggerGameOverEvent += Callback_OnTriggerGameOverEvent;
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent += Callback_OnAbilityChosenLevelUpEvent;


        // PopulateSkillList();
        PopulateAbilityList();
    }

    private void Callback_OnAbilityChosenLevelUpEvent(AbilitySO arg0)
    {
        if (arg0.isOneTimeUse) return;
        UnlockedAbilities.Add(arg0);
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
        UnlockedAbilities ??= new();
        UnlockedAbilities.Clear();
        SetRandomPlanet();
        MenuRoot.SetActive(false);
        GameRoot.SetActive(true);

    }

    private void Callback_OnTriggerGameOverEvent()
    {
        Debug.Log("Callback_OnTriggerGameOverEvent");
    }
    void PopulateAbilityList()
    {
        var loaded = Resources.LoadAll<AbilitySO>("Abilities");
        Abilities = new AbilitySO[loaded.Length];
        for (int i = 0; i < loaded.Length; i++)
        {
            Abilities[i] = ScriptableObject.Instantiate(loaded[i]);
        }


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

    private void OnApplicationQuit()
    {
        _eventChannelGameFlow.OnGamePauseEvent -= Callback_OnGamePauseEvent;
        _eventChannelGameFlow.OnGameUnpauseEvent -= Callback_OnGameUnpauseEvent;
        _eventChannelGameFlow.OnShowMenuEvent -= Callback_OnShowMenuEvent;
        _eventChannelGameFlow.OnNewGameStartEvent -= Callback_OnNewGameStartEvent;
        _eventChannelGameFlow.OnTriggerGameOverEvent -= Callback_OnTriggerGameOverEvent;
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent += Callback_OnAbilityChosenLevelUpEvent;

    }

}
