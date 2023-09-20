using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_InfoGatheredController : MonoBehaviour
{
    [SerializeField] EventChannelScanInfo _eventChannelScanInfo;
    [SerializeField] EventChannelGameFlow _eventChannelGameFlow;


    TMP_Text Label;
    int informationGathered = 0;
    [SerializeField] int InfoGatheredTreshold = 500;

    void OnEnable()
    {
        _eventChannelScanInfo.OnScanInfoLevelChangedEvent += Callback_OnScanInfoLevelChangedEvent;
        Label = GetComponentInChildren<TMP_Text>();
    }


    void OnDisable()
    {
        _eventChannelScanInfo.OnScanInfoLevelChangedEvent -= Callback_OnScanInfoLevelChangedEvent;
    }

    private void Callback_OnScanInfoLevelChangedEvent(int arg0)
    {
        informationGathered += arg0;
        Label.text = $"Information Gathered {informationGathered}/ {InfoGatheredTreshold}Pb";
        // InterfaceManager.Instance.InformationLabel.DOText($"Information Gathered {InformationGathered}/500Pb", 1f, true, ScrambleMode.Lowercase);

        if (informationGathered >= InfoGatheredTreshold)
        {
            _eventChannelGameFlow.RaiseOnTriggerGameOverEvent();
        }
    }
}
