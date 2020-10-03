using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class ShipUIOverlay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Slider HPSlider;
    public Slider IntelSlider;
    public Slider DespairSlider;

    public Text ShipNameText;
    public Text CurrentTaskText;
    public Text CurrentTaskProgressText;

    public GameObject TargetSprite;

    public GameObject HoverObject;


    public ShipController LinkedShip;

    public CircularHealthBarController HealthBar;
    public CircularHealthBarController IntelBar;
    public CircularHealthBarController DespairBar;

    Camera cam;
    void Start()
    {
        cam = Camera.main;
        ShipNameText.text = LinkedShip.ShipName;
        HealthBar.SetNormalizedValue(100f);
        IntelBar.SetNormalizedValue(0f);
        DespairBar.SetNormalizedValue(0f);

    }

    void FixedUpdate()
    {
        transform.position = LinkedShip.transform.position;

        CurrentTaskText.text = LinkedShip.CurrentTask.ToString();
        //HPSlider.normalizedValue = LinkedShip.HP / 100f;
        //IntelSlider.normalizedValue = LinkedShip.Intel / 100f;
        //DespairSlider.normalizedValue = LinkedShip.Despair / 100f;

        HealthBar.SetNormalizedValue(LinkedShip.HP / 100f);
        IntelBar.SetNormalizedValue(LinkedShip.Intel / 100f);
        DespairBar.SetNormalizedValue(LinkedShip.Despair / 100f);
    }

    float timer = 0;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //StartCoroutine(HoverRoutine());
        if (InterfaceManager.Instance.CurrentPointerMode == InterfaceManager.PointerMode.Targeting)
        {
            TargetSprite.SetActive(true);
            //TargetSprite.transform.DOScale(1.2f, 0.5f).SetLoops(1000, LoopType.Yoyo);
            TargetSprite.transform.DOLocalRotate(new Vector3(0, 0, 180), 2f, RotateMode.Fast).SetLoops(10, LoopType.Incremental);
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
            InterfaceManager.Instance.CurrentSelectedSkill.ApplyEffects(LinkedShip);
        }
    }
}
