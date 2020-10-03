using UnityEngine;
using System;

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

    public PointerMode CurrentPointerMode = PointerMode.Default;

    public Skill CurrentSelectedSkill = null;
    public Texture2D TargetingCursorTexture;

    public void SelectSkill(int skillID)
    {
        CurrentPointerMode = PointerMode.Targeting;
        Cursor.SetCursor(TargetingCursorTexture, Vector2.zero, CursorMode.Auto);

        if (skillID == 0)
        {
            CurrentSelectedSkill = GameManager.Instance.SkillList[skillID];
        }
        //Highlight UI
    }

    public void DeselectSkill()
    {
        CurrentPointerMode = PointerMode.Default;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    private void Update()
    {
        if (CurrentPointerMode == PointerMode.Targeting)
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1))
            {
                DeselectSkill();
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

}