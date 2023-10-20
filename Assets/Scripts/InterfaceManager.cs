using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class InterfaceManager : Singleton<InterfaceManager>
{

    [SerializeField] EventChannelAbilities _eventChannelAbilities;
    [SerializeField] EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] Transform overlayParent;

    [SerializeField] Camera _cam;
    public GameObject LeftToolTipPrefab;
    public Skill CurrentSelectedSkill = null;
    public Texture2D TargetingCursorTexture;

    AbilitySO _selectedAbility;

    private PointerMode _currentPointerMode = PointerMode.Default;
    public PointerMode CurrentPointerMode
    {
        get => _currentPointerMode; set
        {
            _currentPointerMode = value;
            switch (_currentPointerMode)
            {
                case PointerMode.Default:
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

                    break;
                case PointerMode.Targeting:
                    Cursor.SetCursor(TargetingCursorTexture, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }

        }
    }
    private void OnEnable()
    {
        _eventChannelAbilities.OnAbilitySelectedEvent += Callback_OnAbilitySelectedEvent;
        _eventChannelAbilities.OnAbilityDeselectedEvent += Callback_OnAbilityDeselectedEvent;
        _eventChannelAbilities.OnAbilityUsedEvent += Callback_OnAbilityUsedEvent;
        _cam = Camera.main;
        _selectedAbility = null;
    }

    private void Callback_OnAbilityUsedEvent(Vector3 arg0, AbilitySO arg1)
    {
        CurrentPointerMode = PointerMode.Default;
        arg1.Effect?.ApplyEffects(arg1, arg0);
        Debug.Log($"Used ability {_selectedAbility}");
    }

    private void Callback_OnAbilityDeselectedEvent()
    {
        CurrentPointerMode = PointerMode.Default;
        Debug.Log($"Deselect ability {_selectedAbility}");
        _selectedAbility = null;
    }

    private void Callback_OnAbilitySelectedEvent(AbilitySO arg0)
    {
        CurrentPointerMode = PointerMode.Targeting;
        _selectedAbility = arg0;
    }

    private void OnDisable()
    {
        _eventChannelAbilities.OnAbilitySelectedEvent -= Callback_OnAbilitySelectedEvent;
        _eventChannelAbilities.OnAbilityDeselectedEvent -= Callback_OnAbilityDeselectedEvent;
        _eventChannelAbilities.OnAbilityUsedEvent -= Callback_OnAbilityUsedEvent;
    }


    // public void SelectSkill(int skillID)
    // {
    //     if (CurrentPointerMode != PointerMode.Default) return;
    //     Debug.Log("Selected skill " + skillID);
    //     AudioManager.PlaySound("click");
    //     if (skillID == 6)
    //     {
    //         GameManager.Instance.SkillList[skillID].ApplyEffects();
    //     }
    //     else
    //     {
    //         selectedButtonIndex = skillID;
    //         CurrentPointerMode = PointerMode.Targeting;
    //         Cursor.SetCursor(TargetingCursorTexture, Vector2.zero, CursorMode.Auto);
    //         CurrentSelectedSkill = GameManager.Instance.SkillList[skillID];
    //     }
    // }

    // public void DeselectSkill()
    // {
    //     // if (LeftButtons[selectedButtonIndex].currentState != UI_ButtonController.ButtonState.CoolDown)
    //     //     LeftButtons[selectedButtonIndex].SetState(UI_ButtonController.ButtonState.Active);
    //     // CurrentPointerMode = PointerMode.Default;
    //     // Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    //     // CurrentSelectedSkill = null;

    // }

    [SerializeField] KeyCode DeselectSkillKeyCode = KeyCode.Escape;
    private void Update()
    {
        if (CurrentPointerMode == PointerMode.Targeting)
        {

            if (Input.GetKeyDown(DeselectSkillKeyCode) || Input.GetMouseButtonDown(1))
            {
                _eventChannelAbilities.RaiseOnSkillDeselectedEvent();
            }

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _eventChannelAbilities.RaiseOnAbilityUsedEvent(_cam.ScreenToWorldPoint(Input.mousePosition), _selectedAbility);
                _eventChannelAbilities.RaiseOnSkillDeselectedEvent();

            }
        }
    }

    public AnimatedLabelPrefabContoller AnimatedLabelPrefab;
    public void ShowAnimatedLabel(Color color, string text, Vector3 pos, bool offset = false)
    {
        Instantiate(AnimatedLabelPrefab, overlayParent).ShowToProgressbar(text, pos);
    }

    public List<UI_ButtonController> LeftButtons = new List<UI_ButtonController>();


    // public GameObject UnlockPanel;

    // public void ShowUnlockPanel(int SkillId)
    // {

    //     UnlockPanel.SetActive(true);

    //     var images = UnlockPanel.GetComponentsInChildren<Image>();
    //     var texts = UnlockPanel.GetComponentsInChildren<Text>();
    //     foreach (var item in images)
    //     {


    //         switch (item.gameObject.name)
    //         {
    //             case "UnlockPanel":
    //                 item.DOFade(0.3f, 0.5f).SetUpdate(true);
    //                 break;
    //             case "Image":
    //                 item.DOFade(1f, 0.5f).SetUpdate(true);
    //                 item.sprite = GameManager.Instance.SkillList[SkillId].icon;
    //                 break;
    //             default:

    //                 item.DOFade(1f, 0.5f).SetUpdate(true);
    //                 break;
    //         }
    //     }
    //     foreach (var item in texts)
    //     {
    //         item.DOFade(1f, 0.5f).SetUpdate(true);
    //         switch (item.gameObject.name)
    //         {
    //             case "SkillNameText":
    //                 item.text = GameManager.Instance.SkillList[SkillId].Name;
    //                 break;
    //             case "DescrText":
    //                 item.text = GameManager.Instance.SkillList[SkillId].Descr;
    //                 break;
    //             case "EffectText":
    //                 item.text = GameManager.Instance.SkillList[SkillId].effectsDescr;
    //                 break;
    //         }
    //     }
    //     LeftButtons[SkillId].UnlockSkill();
    // }

    // public void HideUnlockPanel()
    // {
    //     AudioManager.PlaySound("click");

    //     var images = UnlockPanel.GetComponentsInChildren<Image>();
    //     var texts = UnlockPanel.GetComponentsInChildren<Text>();
    //     foreach (var item in images)
    //     {
    //         item.DOFade(0f, 0.5f);
    //     }
    //     foreach (var item in texts)
    //     {
    //         item.DOFade(0f, 0.5f);
    //     }
    //     UnlockPanel.SetActive(false);

    // }
}
public enum PointerMode
{
    Default, Targeting
}