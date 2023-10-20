using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UIUpgradeChoicePanel : MonoBehaviour
{
    [SerializeField] private EventChannelAbilities _eventChannelAbilities;
    [SerializeField] private EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] private EventChannelGameFlow _eventChannelGameFlow;
    [SerializeField] private EventChannelAudioControl _eventChannelAudioControl;

    [Space]
    [SerializeField] private Image background;
    [SerializeField] private RectTransform panelRect;
    public TMP_Text dataText;
    [Space]
    [SerializeField] private UISkillCardController CardPrefab;
    [SerializeField] private Transform CardParent;

    int defaultNumCards = 3;


    private void OnEnable()
    {
        _eventChannelLevelUp.OnLevelUpEvent += Callback_OnLevelUpEvent;
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent += Callback_OnAbilityChosenLevelUpEvent;
    }


    private void OnDisable()
    {
        _eventChannelLevelUp.OnLevelUpEvent -= Callback_OnLevelUpEvent;
        _eventChannelLevelUp.OnAbilityChosenLevelUpEvent -= Callback_OnAbilityChosenLevelUpEvent;

    }

    private void Callback_OnAbilityChosenLevelUpEvent(AbilitySO arg0)
    {
        Hide();
    }

    private void Callback_OnLevelUpEvent(int arg0)
    {
        Clear();

        List<AbilitySO> eligible = new List<AbilitySO>();
        List<(AbilitySO a, UpgradeableParam p)> eligibleUpgrades = new List<(AbilitySO a, UpgradeableParam p)>();

        foreach (var ability in GameManager.Instance.Abilities)
        {
            if (ability.UnlockLevel > arg0) continue;
            if (GameManager.Instance.UnlockedAbilities.Contains(ability)) continue;

            eligible.Add(ability);
        }

        foreach (var ability in GameManager.Instance.UnlockedAbilities)
        {
            foreach (var uParam in ability.uParamList)
            {
                if (uParam.currentLevel >= uParam.maxLevel) continue;
                eligibleUpgrades.Add((ability, uParam));
            }
        }

        Debug.Log($"eAbilityCount: {eligible.Count} eUpgradesCount:{eligibleUpgrades.Count}");
        // if (eligible.Count > 0)
        // {

        // }
        for (int i = 0; i < defaultNumCards; i++)
        {
            int maxNum = eligibleUpgrades.Count + eligible.Count;
            int index = UnityEngine.Random.Range(0, maxNum);

            if (index < eligible.Count)
            {
                Debug.Log($"choseAbility {index}");
                ShowCard(eligible[index], false);

                eligible.RemoveAt(index);
            }
            else
            {
                Debug.Log($"choseUpgrade {index}");
                ShowCardUpgrade(eligibleUpgrades[index - eligible.Count].a, eligibleUpgrades[index - eligible.Count].p, false);
                eligibleUpgrades.RemoveAt(index - eligible.Count);

            }

        }

        int adIndex = UnityEngine.Random.Range(0, eligibleUpgrades.Count + eligible.Count);

        if (adIndex < eligible.Count)
        {
            Debug.Log($"choseAbility {adIndex}");
            ShowCard(eligible[adIndex], true);
        }
        else
        {
            Debug.Log($"choseUpgrade {adIndex}");
            ShowCardUpgrade(eligibleUpgrades[adIndex - eligible.Count].a, eligibleUpgrades[adIndex - eligible.Count].p, true);
        }


        Debug.Log($"UpgradeChoices -> Eligible count {eligible.Count}/{GameManager.Instance.Abilities.Length}");


        Show();
    }

    float showAnimTime = 0.3f;

    public void Show()
    {
        _eventChannelGameFlow.RaiseOnGamePauseEvent();

        panelRect.gameObject.SetActive(true);
        panelRect.localScale = Vector3.zero;
        panelRect.DOScale(1f, showAnimTime).SetEase(Ease.Linear).SetUpdate(true);
        background.DOFade(0.75f, showAnimTime).SetEase(Ease.Linear).OnComplete(() => background.raycastTarget = true).SetUpdate(true); ;
    }

    public void Hide()
    {
        panelRect.DOScale(0f, showAnimTime).SetEase(Ease.Linear).SetUpdate(true); ;
        background.DOFade(0f, showAnimTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            panelRect.gameObject.SetActive(false);
            background.raycastTarget = false;
            _eventChannelGameFlow.RaiseOnGameUnpauseEvent();
        }).SetUpdate(true); ;
    }

    void Clear()
    {
        for (int i = 0; i < CardParent.childCount; i++)
        {
            Destroy(CardParent.GetChild(i).gameObject);
        }
    }


    public void ShowCard(AbilitySO ability, bool isAd)
    {
        UISkillCardController card = Instantiate(CardPrefab, CardParent);
        card.InitWithData(this, ability, isAd);
    }
    public void ShowCardUpgrade(AbilitySO ability, UpgradeableParam upgrade, bool isAd)
    {
        UISkillCardController card = Instantiate(CardPrefab, CardParent);
        card.InitWithData(this, ability, upgrade, isAd);
    }

}
