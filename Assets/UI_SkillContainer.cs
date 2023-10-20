using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_SkillContainer : MonoBehaviour
{
    [SerializeField] EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] EventChannelGameFlow _eventChannelGameFlow;
    [SerializeField] EventChannelAbilities _eventChannelAbilities;

    [Space]

    [SerializeField] Transform buttonParent;
    [SerializeField] UI_ButtonController buttonPrefab;

    // Dictionary<AbilitySO, UI_ButtonController> buttonDictionary;
    Dictionary<UI_ButtonController, AbilitySO> buttonDictionary;

    void Start()
    {
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent += Callback_OnAbilityChosenLevelUpEvent;
        _eventChannelLevelUp.OnAbilityUpgradedLevelUpEvent += Callback_OnAbilityUpgradedEvent;
        _eventChannelGameFlow.OnNewGameStartEvent += Callback_OnNewGameStartEvent;
    }

    void OnDestroy()
    {
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent -= Callback_OnAbilityChosenLevelUpEvent;
        _eventChannelLevelUp.OnAbilityUpgradedLevelUpEvent -= Callback_OnAbilityUpgradedEvent;

        _eventChannelGameFlow.OnNewGameStartEvent -= Callback_OnNewGameStartEvent;

    }


    private void Callback_OnNewGameStartEvent()
    {
        buttonDictionary ??= new();
        buttonDictionary.Clear();
        _eventChannelLevelUp.RaiseOnAbilityChosenLevelUpEvent(GameManager.Instance.Abilities.Where(a => a.ID == 0).First());

    }

    private void Callback_OnAbilityChosenLevelUpEvent(AbilitySO arg0)
    {
        if (arg0.isOneTimeUse) return;
        CreateButtonFromAbilitySO(arg0);
    }

    private void Callback_OnAbilityUpgradedEvent(AbilitySO arg0, UpgradeableParam arg1)
    {
        UpgradeAbilityButton(arg0, arg1);
    }



    void UpgradeAbilityButton(AbilitySO ability, UpgradeableParam uParam)
    {
        Debug.Log($"UI_SkillContainer -> UpgradeAbilityButton -> {ability.NameKey} {uParam.Name}  lvl{uParam.currentLevel}");


    }
    void CreateButtonFromAbilitySO(AbilitySO ability)
    {
        Debug.Log($"UI_SkillContainer -> CreateButtonFromAbilitySO -> {ability.NameKey}");

        var button = Instantiate(buttonPrefab, buttonParent);
        button.Init(ability);
        buttonDictionary.Add(button, ability);
    }


    [SerializeField] KeyCode UnlockAllSkillsKeyCode = KeyCode.F12;
    [SerializeField] KeyCode Select1keyCode = KeyCode.Alpha1;
    [SerializeField] KeyCode Select2keyCode = KeyCode.Alpha2;
    [SerializeField] KeyCode Select3keyCode = KeyCode.Alpha3;
    [SerializeField] KeyCode Select4keyCode = KeyCode.Alpha4;
    [SerializeField] KeyCode Select5keyCode = KeyCode.Alpha5;
    [SerializeField] KeyCode Select6keyCode = KeyCode.Alpha6;
    [SerializeField] KeyCode Select7keyCode = KeyCode.Alpha7;
    [SerializeField] KeyCode Select8keyCode = KeyCode.Alpha8;
    [SerializeField] KeyCode Select9keyCode = KeyCode.Alpha9;
    [SerializeField] KeyCode Select0keyCode = KeyCode.Alpha0;

    private void Update()
    {

        if (Input.GetKeyDown(Select1keyCode) && transform.childCount == 1)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(0).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select2keyCode) && transform.childCount == 2)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(1).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select3keyCode) && transform.childCount == 3)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(2).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select4keyCode) && transform.childCount == 4)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(3).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select5keyCode) && transform.childCount == 5)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(4).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select6keyCode) && transform.childCount == 6)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(5).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select7keyCode) && transform.childCount == 7)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(6).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select8keyCode) && transform.childCount == 8)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(7).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select9keyCode) && transform.childCount == 9)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(8).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(Select0keyCode) && transform.childCount == 10)
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(buttonDictionary[transform.GetChild(9).GetComponent<UI_ButtonController>()]);
        }
        if (Input.GetKeyDown(UnlockAllSkillsKeyCode))
        {
            var abilities = GameManager.Instance.Abilities;
            foreach (AbilitySO ab in abilities)
            {
                _eventChannelLevelUp.RaiseOnAbilityChosenLevelUpEvent(ab);
            }
        }

    }

    //Temporal anomaly AOE - slows down time for ship
    //Proton storm AOE - disables scanning, adds despair
    //Illusion AOE - stops scanning, military fights, everybody panics
    // Chaos - 
    //Solar flare OTU - sets back all scanning by %
    //Psychic wave OTU - add despair for all ships
    //Aurora Borealis - Reduces scan speed on one side of planet

}
