using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using ProceduralToolkit;
using TMPro;

public class UISkillCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform mRect;
    [SerializeField] Image SkillSpriteImage;
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
    public bool IsUpgrade = false;

    Tween ScaleTween;

    public void InitWithSkillData(AbilitySO skill, bool isUpgrade, bool isAd)
    {
        IsUpgrade = isUpgrade;
        NeedsAd = isAd;
        SkillNameText.text = IsUpgrade ? skill.NameKey + " upgrade" : skill.NameKey;
        SkillSpriteImage.sprite = skill.IconLarge;

        if (IsUpgrade)
        {
            UpgradeArrowsParent.gameObject.SetActive(true);
            StartCoroutine(StartArrowAnimation());
        }
        else
        {
            UpgradeArrowsParent.gameObject.SetActive(false);
        }
        AdIconPersistent.gameObject.SetActive(NeedsAd);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ScaleTween = mRect.DOScale(1.1f, 0.3f);
        GlareEffectImage.DOAnchorPosY(0f, 0.15f);//.OnComplete(() => GlareEffectImage.anchoredPosition = new Vector2(0f, 300f));
        if (NeedsAd)
        {
            // AdTextParent.gameObject.SetActive(true);
            AdFader.DOFade(0.7f, 0.2f);
            AdLabel.DOFade(1f, 0.2f);
            AdIcon.DOFade(1f, 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ScaleTween != null && !ScaleTween.IsComplete())
        {
            ScaleTween.Complete();
        }
        ScaleTween = mRect.DOScale(1f, 0.1f);
        GlareEffectImage.DOAnchorPosY(-300f, 0.15f).OnComplete(() => GlareEffectImage.anchoredPosition = new Vector2(0f, 300f));
        if (NeedsAd)
        {
            AdFader.DOFade(0f, 0.2f);
            AdLabel.DOFade(0f, 0.2f);
            AdIcon.DOFade(0f, 0.2f);
        }
    }



    public IEnumerator StartArrowAnimation()
    {
        float cutoff = 0.33f;
        yield return null;

        for (int i = 0; i < UpgradeArrows.Length; i++)
        {
            // UpgradeArrows[i].color = UpgradeArrows[i].color.WithA(0.5f);
            UpgradeArrows[i].DOFade(1f, cutoff).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            yield return new WaitForSecondsRealtime(cutoff);
            // DOVirtual.DelayedCall(i * cutoff, () => UpgradeArrows[i].DOFade(1f, cutoff).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear));
        }


    }
}
