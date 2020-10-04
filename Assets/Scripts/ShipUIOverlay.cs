using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class ShipUIOverlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Text ShipNameText;
    public Text CurrentTaskText;
    public Text CurrentTaskProgressText;

    public GameObject TargetSprite;

    public GameObject HoverObject;


    public ShipController LinkedShip;

    public CircularHealthBarController HealthBar;
    public CircularHealthBarController IntelBar;
    public CircularHealthBarController DespairBar;

    public Image TimerMask;

    Camera cam;
    void Start()
    {
        //if (GameManager.Instance.inTutorial) return;
        cam = Camera.main;
        ShipNameText.text = LinkedShip.ShipName;
        HealthBar.SetNormalizedValue(100f);
        IntelBar.SetNormalizedValue(0f);
        DespairBar.SetNormalizedValue(0f);

        CurrentTaskText.text = LinkedShip.CurrentTask.ToString();

    }

    void FixedUpdate()
    {
        transform.position = LinkedShip.transform.position;

        //HPSlider.normalizedValue = LinkedShip.HP / 100f;
        //IntelSlider.normalizedValue = LinkedShip.Intel / 100f;
        //DespairSlider.normalizedValue = LinkedShip.Despair / 100f;
        HealthBar.SetNormalizedValue(LinkedShip.HP / 100f);
        IntelBar.SetNormalizedValue(LinkedShip.Intel / 100f);
        DespairBar.SetNormalizedValue(LinkedShip.Despair / 100f);
    }

    public void UpdateTaskText(string text = null)
    {
        if (text == null)
            CurrentTaskText.DOText(LinkedShip.CurrentTask.ToString(), 0.5f, true, ScrambleMode.Lowercase, "1234567890qwertyuiopasdfghjklzxcvbnm");
        else CurrentTaskText.DOText(text, 0.5f, true, ScrambleMode.Lowercase, "1234567890qwertyuiopasdfghjklzxcvbnm");

    }

    float timer = 0;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //StartCoroutine(HoverRoutine());
        if (InterfaceManager.Instance.CurrentPointerMode == InterfaceManager.PointerMode.Targeting)
        {
            TargetSprite.SetActive(true);
            TargetSprite.transform.DOLocalRotate(new Vector3(0, 0, 180), 2f, RotateMode.Fast).SetLoops(-1).SetEase(Ease.Linear);
        }//tween targetsprite
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //HoverObject.SetActive(false);
        TargetSprite.transform.DORestart();
        TargetSprite.SetActive(false);

    }

    IEnumerator HoverRoutine()
    {
        while (timer <= 1f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        HoverObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {


    }
    public void OnClicked()
    {
        //Debug.Log(InterfaceManager.Instance.CurrentPointerMode);
        if (InterfaceManager.Instance.CurrentPointerMode == InterfaceManager.PointerMode.Targeting)
        {
            Debug.Log($"clicked {LinkedShip.ShipName}. Applying {InterfaceManager.Instance.CurrentSelectedSkill}");
            InterfaceManager.Instance.LeftButtons[InterfaceManager.Instance.CurrentSelectedSkill.ID].SetState(UI_ButtonController.ButtonState.CoolDown);
            Debug.Log($"SkillID{InterfaceManager.Instance.CurrentSelectedSkill.ID}");
            InterfaceManager.Instance.CurrentSelectedSkill.ApplyEffects(LinkedShip);
            InterfaceManager.Instance.DeselectSkill();
        }
    }

    public IEnumerator TimerMaskCoroutine(float time)
    {
        Debug.Log("TimerMask");
        TimerMask.gameObject.SetActive(true);
        float fraction = time / 360;
        while (TimerMask.fillAmount >= 0)
        {
            TimerMask.fillAmount -= fraction;
            yield return new WaitForSeconds(1f);
        }
        TimerMask.fillAmount = 360f;
        TimerMask.gameObject.SetActive(false);
    }
}
