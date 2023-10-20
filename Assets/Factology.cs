using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factology : Singleton<Factology>
{
    [SerializeField] EventChannelFactology _eventChannelFactology;

    Dictionary<string, bool> _facts;
    // Start is called before the first frame update
    void Start()
    {
        _facts = new Dictionary<string, bool>();
    }

    void OnEnable()
    {
        _eventChannelFactology.TriggerFactEvent += Callback_OnTriggerFactEvent;
    }
    void OnDisable()
    {
        _eventChannelFactology.TriggerFactEvent -= Callback_OnTriggerFactEvent;
    }

    private void Callback_OnTriggerFactEvent(string arg0, bool arg1)
    {
        if (!_facts.ContainsKey(arg0))
        {
            _facts.Add(arg0, arg1);
            return;
        }
        if (_facts[arg0] == arg1)
        {
            Debug.LogWarning($"[<colot=green>Factology</color>] -> Fact {arg0} set to same value {arg1}");
            return;
        }

        _facts[arg0] = arg1;
    }

    public bool GetFact(string id)
    {
        if (!_facts.ContainsKey(id))
        {
            _facts.Add(id, false);
        }
        return _facts[id];
    }
}
