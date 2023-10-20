using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UI_DBG_ShipOverlay : MonoBehaviour
{
    [SerializeField] private TMP_Text NameLabel;
    [SerializeField] private TMP_Text TaskLabel;
    [SerializeField] private TMP_Text HPLabel;
    [SerializeField] private TMP_Text DespairLabel;
    [SerializeField] private TMP_Text InfoLabel;
    [SerializeField] private TMP_Text StatusLabel;

    RectTransform _rect;
    Transform _shipTransform;
    Camera _cam;

    public void SetNameLabel(string text) => NameLabel.text = text;
    public void SetTaskLabel(string text) => TaskLabel.text = text;
    public void SetHPLabel(string text) => HPLabel.text = text;
    public void SetDespairLabel(string text) => DespairLabel.text = text;
    public void SetInfoLabel(string text) => InfoLabel.text = text;
    public void SetStatusLabel(string text) => StatusLabel.text = text;

    internal void Init(ShipController newShipController, Camera camera)
    {
        _rect = GetComponent<RectTransform>();
        _shipTransform = newShipController.transform;
        _cam = camera;

    }

    void LateUpdate()
    {
        if (_shipTransform == null) return;
        transform.position = _cam.WorldToScreenPoint(_shipTransform.position);

        // _rect.anchoredPosition = RectTransformUtility.WorldToScreenPoint(_cam, _shipTransform.position);
    }
}
