using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class ProgressManager : Singleton<ProgressManager>
{

    [SerializeField] EventChannelDespairScoreTick _eventChannelDespairScoreTick;
    [SerializeField] EventChannelGameFlow _eventChannelGameFlow;
    [SerializeField] EventChannelLevelUp _eventChannelLevelUp;
    [SerializeField] EventChannelAbilities _eventChannelAbilities;


    [SerializeField] Slider ProgressSlider;

    [SerializeField] string progressTextTail;
    [SerializeField] TMP_Text CurrentProgressLabel;
    [SerializeField] TMP_Text MaxProgressLabel;
    [SerializeField] TMP_Text MinProgressLabel;
    [SerializeField] TMP_Text LevelLabel;

    [SerializeField] RectTransform LabelTweenTarget;
    [SerializeField] Camera _Cam;

    Dictionary<int, (int, int)> ProgressDictionary = new Dictionary<int, (int, int)>();

    private int _score = 0;
    int Level = 0;
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

    void Start()
    {
        Debug.Log("PM Start");
        _eventChannelDespairScoreTick.OnDespairTickEvent += CallBack_OnDespairTickEvent;
        _eventChannelGameFlow.OnNewGameStartEvent += CallBack_OnNewGameStartEvent;
    }


    void OnDestroy()
    {
        _eventChannelDespairScoreTick.OnDespairTickEvent -= CallBack_OnDespairTickEvent;
        _eventChannelGameFlow.OnNewGameStartEvent -= CallBack_OnNewGameStartEvent;

    }
    private void CallBack_OnNewGameStartEvent()
    {
        Level = 0;
        LevelUP(true);
    }



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
            CurrentProgressLabel.text = $"{_score}";

        }

    }


    public Vector2 GetTweenTargetPosition() => LabelTweenTarget.position;
    void LevelUP(bool silent = false)
    {
        Level++;
        Debug.Log($"LVLUp called {Level}");
        LevelLabel.text = $"lvl {Level}";
        var (_min, _max) = ProgressDictionary[Level];
        ProgressSlider.minValue = _min;
        MinProgressLabel.text = $"{_min}";
        MaxProgressLabel.text = $"{_max}";
        CurrentProgressLabel.text = $"{_score}";
        ProgressSlider.maxValue = _max;
        ProgressSlider.DOValue(_score, 0.5f);

        if (!silent) _eventChannelLevelUp.RaiseOnLevelUpEvent(Level);
    }

}
