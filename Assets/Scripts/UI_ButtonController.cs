using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UI_ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ButtonState
    {
        Locked, CoolDown, Active, Selected
    }

    public ButtonState currentState = ButtonState.Locked;

    public Image bg;
    public Image frame_active;
    public Image frame_selected;
    public Image overlay_locked;
    public Image overlay_masked;
    public Image mask;
    public Text cd_text;

    static GameObject ToolTip;

    public int skillID;
    public Skill skill;


    public float hoverCd = 2;
    void Start()
    {
        StartCoroutine(InitRoutine());
    }

    public void SetState(ButtonState newState)
    {
        switch (newState)
        {
            case ButtonState.Locked:

                frame_active.gameObject.SetActive(false);
                overlay_locked.gameObject.SetActive(true);
                overlay_masked.gameObject.SetActive(false);
                frame_selected.gameObject.SetActive(false);

                cd_text.gameObject.SetActive(false);

                break;
            case ButtonState.CoolDown:
                frame_active.gameObject.SetActive(false);
                overlay_locked.gameObject.SetActive(false);
                overlay_masked.gameObject.SetActive(true);
                frame_selected.gameObject.SetActive(false);
                cd_text.gameObject.SetActive(true);
                StartCoroutine(CooldownRoutine());

                break;
            case ButtonState.Active:
                Debug.Log("VOT TI GDE PIDAR");

                frame_active.gameObject.SetActive(true);
                overlay_locked.gameObject.SetActive(false);
                overlay_masked.gameObject.SetActive(false);
                frame_selected.gameObject.SetActive(false);
                cd_text.gameObject.SetActive(false);

                break;
            case ButtonState.Selected:
                frame_active.gameObject.SetActive(false);
                overlay_locked.gameObject.SetActive(false);
                overlay_masked.gameObject.SetActive(false);
                frame_selected.gameObject.SetActive(true);
                cd_text.gameObject.SetActive(false);

                break;
        }
        currentState = newState;
    }



    IEnumerator InitRoutine()
    {
        yield return null;
        skill = GameManager.Instance.SkillList[skillID];
        SetState(ButtonState.Locked);
        if (skill.ID == 0) UnlockSkill();
    }
    IEnumerator CooldownRoutine()
    {

        Debug.Log("CD ROUTINE");
        for (int i = 0; i < skill.cooldown; i++)
        {
            cd_text.text = $"{skill.cooldown - i} s";
            //TWEEN MASK
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("CD ROUTINE2");
        SetState(ButtonState.Active);
    }

    bool stopped = false;
    bool shown = false;
    IEnumerator HoverRoutine()
    {
        tooltipShown = true;
        yield return new WaitForSeconds(hoverCd);

        Vector3 pos = new Vector3(transform.position.x + 300, transform.position.y, 0);

        DestroyImmediate(ToolTip);
        if (!isPointerInSkill) yield break;
        ToolTip = Instantiate(InterfaceManager.Instance.LeftToolTipPrefab, pos, transform.rotation);
        ToolTip.transform.SetParent(transform.parent.parent);
        var texts = ToolTip.GetComponentsInChildren<Text>();
        foreach (var item in texts)
        {
            if (item.gameObject.name == "title")
            {
                item.text = skill.Name;
            }
            if (item.gameObject.name == "descr")
            {
                item.text = skill.Descr;
            }
            if (item.gameObject.name == "effects")
            {
                item.text = skill.effectsDescr;
            }
        }
        ToolTip.SetActive(true);
    }

    public void UnlockSkill()
    {
        SetState(ButtonState.Active);
        transform.DOShakePosition(1f);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.PlaySound("click");

        switch (currentState)
        {
            case ButtonState.Locked:
                //overlay_locked.DOColor(Color.red, 0.5f).SetLoops(1, LoopType.Yoyo).SetEase(Ease.Linear);

                break;
            case ButtonState.CoolDown:
                //overlay_locked.DOColor(Color.red, 0.5f).SetLoops(1, LoopType.Yoyo).SetEase(Ease.Linear);

                break;
            case ButtonState.Active:
                Debug.Log("clock button");
                InterfaceManager.Instance.SelectSkill(skill.ID);
                SetState(ButtonState.Selected);
                break;
            case ButtonState.Selected:
                InterfaceManager.Instance.DeselectSkill();
                SetState(ButtonState.Active);
                break;
        }
    }
    static bool tooltipShown = false;
    bool isPointerInSkill = false;

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInSkill = false;

        Debug.Log($"OPEx ---> {skill.Name}");
        if (tooltipShown) HideToolTip();

    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isPointerInSkill = true;
        Debug.Log($"OPEn ---> {skill.Name}");
        if (!tooltipShown)
            if (currentState != ButtonState.Locked)
            {
                StartCoroutine(HoverRoutine());
            }
    }

    void HideToolTip()
    {
        StopCoroutine(HoverRoutine());
        DestroyImmediate(ToolTip);
        tooltipShown = false;
    }
}