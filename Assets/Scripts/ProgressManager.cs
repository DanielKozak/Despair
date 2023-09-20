using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressManager : MonoBehaviour
{

    [SerializeField] EventChannelDespairScoreTick _eventChannelDespairScoreTick;
    [SerializeField] EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] EventChannelAbilities _eventChannelAbilities;

    private void Awake()
    {
        ProgressDictionary.Add(1, (0, 50));
        ProgressDictionary.Add(2, (50, 250));
        ProgressDictionary.Add(3, (250, 500));
        ProgressDictionary.Add(4, (500, 1000));
        ProgressDictionary.Add(5, (1000, 1500));
        ProgressDictionary.Add(6, (1500, 2500));
        ProgressDictionary.Add(7, (2500, 5000));
        ProgressDictionary.Add(8, (5000, 7500));
        ProgressDictionary.Add(9, (7500, 10000));
    }

    void OnEnable()
    {
        _eventChannelDespairScoreTick.OnDespairTickEvent += CallBack_OnDespairTickEvent;

    }



    void OnDisable()
    {
        _eventChannelDespairScoreTick.OnDespairTickEvent -= CallBack_OnDespairTickEvent;

    }


    [SerializeField] Slider ProgressSlider;

    [SerializeField] string progressTextTail;
    [SerializeField] TMP_Text CurrentProgressLabel;
    [SerializeField] TMP_Text MaxProgressLabel;
    [SerializeField] TMP_Text MinProgressLabel;
    [SerializeField] TMP_Text LevelLabel;

    Dictionary<int, (int, int)> ProgressDictionary = new Dictionary<int, (int, int)>();

    private int _score = 0;


    int Level = 1;
    private void CallBack_OnDespairTickEvent(int arg0)
    {
        _score += arg0;
        CheckScoreAgainstLevel(_score);
    }
    public void CheckScoreAgainstLevel(int score)
    {
        var (min, max) = ProgressDictionary[Level];
        // if (score % 100 == 0 && Random.Range(0f, 1f) >= 0.5f) GameManager.Instance.GenerateShip();
        if (score >= max)
        {
            LevelUP();

        }
        else
        {
            ProgressSlider.DOValue(score, 0.5f);
        }

    }

    public void LevelUP()
    {
        Level++;
        LevelLabel.text = $"lvl {Level}";
        var (_min, _max) = ProgressDictionary[Level];
        ProgressSlider.minValue = _min;
        MinProgressLabel.text = $"{_min}";
        MaxProgressLabel.text = $"{_max}";
        ProgressSlider.maxValue = _max;
        ProgressSlider.DOValue(_score, 0.5f);

        _eventChannelLevelUp.RaiseOnLevelUpEvent(Level);
    }

}
