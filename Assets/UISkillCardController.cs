using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using ProceduralToolkit;
using TMPro;

public class UISkillCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] RectTransform mRect;
    [SerializeField] Image SkillSpriteImage;
    [SerializeField] Image SkillSpriteImageOverlay;
    [SerializeField] TMP_Text SkillNameText;
    [SerializeField] TMP_Text SkillDesctiptionText;

    [SerializeField] RectTransform GlareEffectImage;
    [SerializeField] RectTransform UpgradeArrowsParent;
    [SerializeField] Image[] UpgradeArrows;


    [SerializeField] Image AdFader;
    [SerializeField] RectTransform AdIconPersistent;
    [SerializeField] TMP_Text AdLabel;
    [SerializeField] Image AdIcon;

    public bool NeedsAd = false;

    bool isUpgrade = false;

    Tween ScaleTween;
    AbilitySO Ability;
    UpgradeableParam uParam;

    UIUpgradeChoicePanel _panel;

    public void InitWithData(UIUpgradeChoicePanel panel, AbilitySO ability, bool isAd)
    {
        _eventChannelLevelUp = Resources.Load<EventChannelLevelUp>("EventChannels/EventChannelLevelUp");
        Ability = ability;
        _panel = panel;
        NeedsAd = isAd;
        SkillNameText.text = Ability.NameKey;
        SkillSpriteImage.sprite = Ability.IconLarge;
        SkillSpriteImageOverlay.gameObject.SetActive(false);

        SkillDesctiptionText.text = ability.DescriptionKey;
        UpgradeArrowsParent.gameObject.SetActive(false);

        AdIconPersistent.gameObject.SetActive(NeedsAd);
    }
    public void InitWithData(UIUpgradeChoicePanel panel, AbilitySO ability, UpgradeableParam upgrade, bool isAd)
    {
        _eventChannelLevelUp = Resources.Load<EventChannelLevelUp>("EventChannels/EventChannelLevelUp");
        Ability = ability;
        uParam = upgrade;
        _panel = panel;

        NeedsAd = isAd;
        SkillNameText.text = $"{Ability.NameKey} \n<color=yellow>upgrade</color>";
        SkillSpriteImage.sprite = Ability.IconLarge;
        SkillSpriteImageOverlay.sprite = upgrade.Icon;
        SkillDesctiptionText.text = ability.DescriptionKey;

        UpgradeArrowsParent.gameObject.SetActive(true);
        // StartCoroutine(StartArrowAnimation());
        isUpgrade = true;
        AdIconPersistent.gameObject.SetActive(NeedsAd);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTween = mRect.DOScale(1.1f, 0.3f).SetUpdate(true);
        GlareEffectImage.DOAnchorPosY(0f, 0.15f).SetUpdate(true); //.OnComplete(() => GlareEffectImage.anchoredPosition = new Vector2(0f, 300f));
        if (NeedsAd)
        {
            // AdTextParent.gameObject.SetActive(true);
            AdFader.DOFade(0.7f, 0.2f).SetUpdate(true);
            AdLabel.DOFade(1f, 0.2f).SetUpdate(true);
            AdIcon.DOFade(1f, 0.2f).SetUpdate(true);
        }
        // Debug.Log($"<color=red>{_panel}</color>");
        if (isUpgrade)
        {
            _panel.dataText.text = $"{uParam.Description} from <color=red>{Ability.GetParam(uParam.Name).value}</color> to <color=green>{uParam.value}</color>";
        }
        else
        {
            _panel.dataText.text = Ability.DescriptionKey;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ScaleTween != null && !ScaleTween.IsComplete())
        {
            ScaleTween.Complete();
        }
        ScaleTween = mRect.DOScale(1f, 0.1f).SetUpdate(true);
        GlareEffectImage.DOAnchorPosY(-300f, 0.15f).OnComplete(() => GlareEffectImage.anchoredPosition = new Vector2(0f, 300f)).SetUpdate(true);
        if (NeedsAd)
        {
            AdFader.DOFade(0f, 0.2f).SetUpdate(true);
            AdLabel.DOFade(0f, 0.2f).SetUpdate(true);
            AdIcon.DOFade(0f, 0.2f).SetUpdate(true);
        }
        _panel.dataText.text = "";
    }



    public IEnumerator StartArrowAnimation()
    {
        float cutoff = 0.33f;
        yield return null;

        for (int i = 0; i < UpgradeArrows.Length; i++)
        {
            // UpgradeArrows[i].color = UpgradeArrows[i].color.WithA(0.5f);
            UpgradeArrows[i].DOFade(1f, cutoff).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetUpdate(true);
            yield return new WaitForSecondsRealtime(cutoff);
            // DOVirtual.DelayedCall(i * cutoff, () => UpgradeArrows[i].DOFade(1f, cutoff).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear));
        }


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"AbilityCard -> OnPointerClick()");
        if (!isUpgrade)
        {
            _eventChannelLevelUp.RaiseOnAbilityChosenLevelUpEvent(Ability);
        }
        else
        {
            _eventChannelLevelUp.RaiseOnAbilityUpgradedLevelUpEvent(Ability, uParam);
        }

        // _eventChannelLevelUp.RaiseOnAbilityChosenLevelUpEvent(Ability);
    }
}
