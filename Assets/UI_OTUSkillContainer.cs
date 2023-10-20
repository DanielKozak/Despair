using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_OTUSkillContainer : MonoBehaviour
{
    [SerializeField] EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] EventChannelGameFlow _eventChannelGameFlow;
    [SerializeField] EventChannelAbilities _eventChannelAbilities;

    [Space]

    [SerializeField] Transform buttonParent;
    [SerializeField] UI_OTUButtonController buttonPrefab;

    // Dictionary<AbilitySO, UI_ButtonController> buttonDictionary;
    Dictionary<AbilitySO, UI_OTUButtonController> buttonDictionary;

    void Start()
    {
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent += Callback_OnAbilityChosenLevelUpEvent;
        _eventChannelGameFlow.OnNewGameStartEvent += Callback_OnNewGameStartEvent;
    }

    void OnDestroy()
    {
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent -= Callback_OnAbilityChosenLevelUpEvent;
        _eventChannelGameFlow.OnNewGameStartEvent -= Callback_OnNewGameStartEvent;
    }


    private void Callback_OnNewGameStartEvent()
    {
        buttonDictionary ??= new();
        buttonDictionary.Clear();
    }

    private void Callback_OnAbilityChosenLevelUpEvent(AbilitySO arg0)
    {
        if (!arg0.isOneTimeUse) return;
        CreateButtonFromAbilitySO(arg0);
    }



    void CreateButtonFromAbilitySO(AbilitySO ability)
    {
        Debug.Log($"UI_OTUSkillContainer -> CreateButtonFromAbilitySO -> {ability.NameKey}");
        if (buttonDictionary.ContainsKey(ability))
        {
            buttonDictionary[ability].NumUses += 1;
        }
        else
        {
            UI_OTUButtonController button = Instantiate(buttonPrefab, buttonParent);
            button.Init(ability);
            buttonDictionary.Add(ability, button);
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
