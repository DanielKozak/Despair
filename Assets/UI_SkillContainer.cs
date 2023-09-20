using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_SkillContainer : MonoBehaviour
{
    [SerializeField] EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] EventChannelGameFlow _eventChannelGameFlow;

    [Space]

    [SerializeField] Transform buttonParent;
    [SerializeField] UI_ButtonController buttonPrefab;

    Dictionary<AbilitySO, UI_ButtonController> buttonDictionary;

    void OnEnable()
    {
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent += Callback_OnAbilityChosenLevelUpEvent;
        _eventChannelGameFlow.OnNewGameStartEvent += Callback_OnNewGameStartEvent;
    }

    void OnDisable()
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
        _ = CreateButtonFromAbilitySO(arg0);
    }




    UI_ButtonController CreateButtonFromAbilitySO(AbilitySO ability)
    {
        if (buttonDictionary.ContainsKey(ability.ReplacesAbility))
        {
            Destroy(buttonDictionary[ability.ReplacesAbility].gameObject);
            buttonDictionary.Remove(ability.ReplacesAbility);
        }
        var button = Instantiate(buttonPrefab, buttonParent);

        return button;
    }
}
