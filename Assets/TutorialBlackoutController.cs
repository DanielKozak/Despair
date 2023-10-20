using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class TutorialBlackoutController : MonoBehaviour
{
    [SerializeField] EventChannelTutorial _eventChannelTutorial;
    [SerializeField] EventChannelFactology _eventChannelFactology;
    [SerializeField] EventChannelGameFlow _eventChannelGameFlow;
    [Space]

    [SerializeField] GameObject _uiContainer;
    [SerializeField] TMP_Text _label;
    [SerializeField] Image _blackoutImage;

    [SerializeField] Camera _mCam;
    Vector2[] _labelAnchors = new Vector2[] { Vector2.zero, Vector2.up, Vector2.one, Vector2.right };

    Material imageMaterialReference;
    RectTransform textRectTransform;


    bool isShown = false;
    string _id;

    void OnEnable()
    {
        _eventChannelTutorial.ShowTutorialMessageEvent += Callback_OnShowTutorialMessage;
        _eventChannelTutorial.ConfirmTutorialMessageEvent += Callback_OnConfirmTutorialMessage;

        imageMaterialReference = _label.material;
        textRectTransform = _label.GetComponent<RectTransform>();
    }

    void OnDisable()
    {
        _eventChannelTutorial.ShowTutorialMessageEvent -= Callback_OnShowTutorialMessage;
        _eventChannelTutorial.ConfirmTutorialMessageEvent -= Callback_OnConfirmTutorialMessage;
    }
    private void Callback_OnShowTutorialMessage(string arg0, Vector2 arg1, string arg2)
    {
        QueryTutorialSystem(arg0, arg1, arg2);
    }

    private void Callback_OnConfirmTutorialMessage(string arg0)
    {
        HideTutorial(arg0);
    }

    void QueryTutorialSystem(string arg0, Vector2 arg1, string arg2)
    {
        if (!Factology.Instance.GetFact("tutorial_isEnabled"))
        {
            return;
        }

        if (Factology.Instance.GetFact(arg0))
        {
            return;
        }

        ShowTutorial(arg0, arg1, arg2);
    }


    void ShowTutorial(string id, Vector2 screenPos, string textKey)
    {
        _id = id;
        Vector2 viewportPos = _mCam.ScreenToViewportPoint(screenPos);

        if (viewportPos.x < 0.5f && viewportPos.y < 0.5f)
        {
            Debug.Log(0);

            textRectTransform.anchorMin = textRectTransform.anchorMax = textRectTransform.pivot = _labelAnchors[2];
        }
        else if (viewportPos.x < 0.5f && viewportPos.y > 0.5f)
        {
            Debug.Log(1);

            textRectTransform.anchorMin = textRectTransform.anchorMax = textRectTransform.pivot = _labelAnchors[3];
        }
        else if (viewportPos.x > 0.5f && viewportPos.y < 0.5f)
        {
            Debug.Log(2);

            textRectTransform.anchorMin = textRectTransform.anchorMax = textRectTransform.pivot = _labelAnchors[1];
        }
        else if (viewportPos.x > 0.5f && viewportPos.y > 0.5f)
        {
            Debug.Log(3);

            textRectTransform.anchorMin = textRectTransform.anchorMax = textRectTransform.pivot = _labelAnchors[0];
        }

        _label.text = textKey;

        isShown = true;

        var vector = new Vector4(viewportPos.x, viewportPos.y, 0f, 0f);
        Debug.Log(vector);

        _uiContainer.SetActive(true);
        _uiContainer.SetActive(true);
        _eventChannelGameFlow.RaiseOnGamePauseEvent();

    }

    void HideTutorial(string id)
    {

        isShown = false;
        _uiContainer.SetActive(false);
        _eventChannelGameFlow.RaiseOnGameUnpauseEvent();
        _eventChannelFactology.RaiseTriggerFactEvent(id, true);
    }

    void Update()
    {
        if (!isShown)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Return))
        {
            _eventChannelTutorial.RaiseConfirmTutorialMessageEvent(_id);
        }
    }




}
