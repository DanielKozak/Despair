using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

public class UI_ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    EventChannelAbilities _eventChannelAbilities;

    [SerializeField] Image bg;
    [SerializeField] Image frame_selected;
    [SerializeField] Image overlay_masked;
    [SerializeField] Text cd_text;

    AbilitySO _ability;


    public float hoverCd = 2;

    void OnEnable()
    {
        _eventChannelAbilities ??= RuntimeEventChannelContainer.Instance.EventChannelAbilitiesInstance;

        _eventChannelAbilities.OnAbilityUsedEvent += Callback_OnAbilityUsed;
        _eventChannelAbilities.OnAbilitySelectedEvent += Callback_OnAbilitySelectedEvent;
        _eventChannelAbilities.OnAbilityDeselectedEvent += Callback_OnAbilityDeselectedEvent;
    }


    void OnDisable()
    {
        _eventChannelAbilities.OnAbilityUsedEvent -= Callback_OnAbilityUsed;
        _eventChannelAbilities.OnAbilitySelectedEvent -= Callback_OnAbilitySelectedEvent;
        _eventChannelAbilities.OnAbilityDeselectedEvent -= Callback_OnAbilityDeselectedEvent;
    }

    void Select()
    {

        frame_selected.gameObject.SetActive(true);
        // frame_selected.D

        DOTween.To(() => frame_selected.pixelsPerUnitMultiplier, x => frame_selected.pixelsPerUnitMultiplier = x, 2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        // frame_selected.DOFade(0.8f, 0.5f).SetLoops(-1).SetEase(Ease.Linear);
    }

    void Deselect()
    {
        frame_selected.DOKill();
        frame_selected.gameObject.SetActive(false);
    }

    private void Callback_OnAbilityDeselectedEvent()
    {
        Deselect();
    }

    private void Callback_OnAbilitySelectedEvent(AbilitySO arg0)
    {
        if (!arg0.Equals(_ability))
        {
            Deselect();
            return;
        }

        Select();

    }

    private void Callback_OnAbilityUsed(Vector3 arg0, AbilitySO arg1)
    {
        if (!arg1.Equals(_ability)) return;

        OnUsed();
    }

    public void Init(AbilitySO ability)
    {
        // transform.DOShakePosition(1f);
        _ability = ability;
        bg.sprite = _ability.IconSmall;
        GetComponent<Button>().onClick.AddListener(() => OnSelect());

    }

    public void AddUpgradeMarker(UpgradeableParam uParam)
    {

    }



    bool isInCoolDown = false;
    private void OnSelect()
    {
        if (isInCoolDown)
        {
            return;
        }


        if (_ability.isUseImmediate)
        {
            _eventChannelAbilities.RaiseOnAbilityUsedEvent(Vector3.zero, _ability);

        }
        else
        {
            _eventChannelAbilities.RaiseOnSkillSelectedEvent(_ability);
        }
    }

    private void OnUsed()
    {
        isInCoolDown = true;

        overlay_masked.fillAmount = 1;
        overlay_masked.DOFillAmount(0f, _ability.GetParam("cooldown").value).SetEase(Ease.Linear).OnComplete(() => isInCoolDown = false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("ShowTooltip");
        ToolTipController.Instance.Show(_ability);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("HideTooltip");

        ToolTipController.Instance.Hide();
    }

}