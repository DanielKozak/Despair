using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System;

public class UIUpgradeChoicePanel : MonoBehaviour
{
    [SerializeField] private EventChannelAbilities _eventChannelAbilities;
    [SerializeField] private EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] private EventChannelGameFlow _eventChannelGameFlow;
    [SerializeField] private EventChannelAudioControl _eventChannelAudioControl;

    [Space]
    [SerializeField] private Image background;
    [SerializeField] private RectTransform panelRect;
    [Space]
    [SerializeField] private UISkillCardController CardPrefab;
    [SerializeField] private Transform CardParent;


    private void OnEnable()
    {
        _eventChannelLevelUp.OnLevelUpEvent += OnLevelUpEvent;
    }


    private void OnDisable()
    {
        _eventChannelLevelUp.OnLevelUpEvent -= OnLevelUpEvent;
    }
    private void OnLevelUpEvent(int arg0)
    {
        Show();
    }

    float showAnimTime = 0.3f;

    [ContextMenu("show")]
    public void Show()
    {
        _eventChannelGameFlow.RaiseOnGamePauseEvent();

        panelRect.gameObject.SetActive(true);
        panelRect.localScale = Vector3.zero;
        panelRect.DOScale(1f, showAnimTime).SetEase(Ease.Linear);
        background.DOFade(0.75f, showAnimTime).SetEase(Ease.Linear).OnComplete(() => background.raycastTarget = true);
    }

    [ContextMenu("hide")]
    public void Hide()
    {
        panelRect.DOScale(0f, showAnimTime).SetEase(Ease.Linear);
        background.DOFade(0f, showAnimTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            panelRect.gameObject.SetActive(false);
            background.raycastTarget = false;
            _eventChannelGameFlow.RaiseOnGameUnpauseEvent();
        });
    }


    public UISkillCardController ShowCard(AbilitySO skill, bool isUpgrade, bool isAd)
    {
        var card = Instantiate(CardPrefab, CardParent);
        card.InitWithSkillData(skill, isUpgrade, isAd);
        return card;
    }

}
