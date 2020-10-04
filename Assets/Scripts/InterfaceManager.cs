using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public enum PointerMode
    {
        Default, Targeting
    }
    public GameObject LeftToolTipPrefab;

    public PointerMode CurrentPointerMode = PointerMode.Default;

    public Skill CurrentSelectedSkill = null;
    public Texture2D TargetingCursorTexture;

    public int selectedButtonIndex = -1;
    public void SelectSkill(int skillID)
    {
        Debug.Log("Selected skill " + skillID);
        if (skillID == 6)
        {
            GameManager.Instance.SkillList[skillID].ApplyEffects();
        }
        else
        {
            selectedButtonIndex = skillID;
            CurrentPointerMode = PointerMode.Targeting;
            Cursor.SetCursor(TargetingCursorTexture, Vector2.zero, CursorMode.Auto);
            CurrentSelectedSkill = GameManager.Instance.SkillList[skillID];
        }
        selectedButtonIndex = skillID;
        CurrentPointerMode = PointerMode.Targeting;
        Cursor.SetCursor(TargetingCursorTexture, Vector2.zero, CursorMode.Auto);
        CurrentSelectedSkill = GameManager.Instance.SkillList[skillID];
    }

    public void DeselectSkill()
    {
        if (LeftButtons[selectedButtonIndex].currentState != UI_ButtonController.ButtonState.CoolDown)
            LeftButtons[selectedButtonIndex].SetState(UI_ButtonController.ButtonState.Active);
        CurrentPointerMode = PointerMode.Default;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        CurrentSelectedSkill = null;

    }
    private void Update()
    {
        //if (GameManager.Instance.CurrentGameState != GameManager.GameState.Playing) return;
        if (CurrentPointerMode == PointerMode.Targeting)
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
            {
                DeselectSkill();
            }
            if (Input.GetMouseButtonUp(0))
            {
                //DeselectSkill();
            }
        }
    }

    public GameObject AnimatedLabelPrefab;
    public void ShowAnimatedLabel(Color color, string text, Vector3 pos, bool offset = false)
    {
        var lab = Instantiate(AnimatedLabelPrefab, GameManager.Instance.LabelParent.transform);
        Vector3 position = offset ? new Vector3(pos.x + 0.1f, pos.y + 0.1f, 0) : pos;
        lab.GetComponent<AnimatedLabelPrefabContoller>().Show(color, text, position);
    }

    public List<UI_ButtonController> LeftButtons = new List<UI_ButtonController>();

    public Text InformationLabel;
    public GameObject UnlockPanel;

    public void ShowUnlockPanel(int SkillId)
    {

        UnlockPanel.SetActive(true);
        GameManager.Instance.CurrentGameState = GameManager.GameState.Paused;

        var images = UnlockPanel.GetComponentsInChildren<Image>();
        var texts = UnlockPanel.GetComponentsInChildren<Text>();
        foreach (var item in images)
        {


            switch (item.gameObject.name)
            {
                case "UnlockPanel":
                    item.DOFade(0.3f, 0.5f).SetUpdate(true);
                    break;
                case "Image":
                    item.DOFade(1f, 0.5f).SetUpdate(true);
                    item.sprite = GameManager.Instance.SkillList[SkillId].icon;
                    break;
                default:

                    item.DOFade(1f, 0.5f).SetUpdate(true);
                    break;
            }
        }
        foreach (var item in texts)
        {
            item.DOFade(1f, 0.5f).SetUpdate(true);
            switch (item.gameObject.name)
            {
                case "SkillNameText":
                    item.text = GameManager.Instance.SkillList[SkillId].Name;
                    break;
                case "DescrText":
                    item.text = GameManager.Instance.SkillList[SkillId].Descr;
                    break;
                case "EffectText":
                    item.text = GameManager.Instance.SkillList[SkillId].effectsDescr;
                    break;
            }
        }
        LeftButtons[SkillId].UnlockSkill();
    }

    public void HideUnlockPanel()
    {
        AudioManager.PlaySound("click");

        var images = UnlockPanel.GetComponentsInChildren<Image>();
        var texts = UnlockPanel.GetComponentsInChildren<Text>();
        foreach (var item in images)
        {
            item.DOFade(0f, 0.5f);
        }
        foreach (var item in texts)
        {
            item.DOFade(0f, 0.5f);
        }
        UnlockPanel.SetActive(false);
        GameManager.Instance.CurrentGameState = GameManager.GameState.Playing;

    }
}