using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProgressManager : MonoBehaviour
{

    public static ProgressManager Instance;
    private void Awake()
    {
        Instance = this;
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


    public Slider ProgressSlider;

    public string progressTextTail;
    public Text ProgressLabel;
    public Text LevelLabel;

    public Dictionary<int, (int, int)> ProgressDictionary = new Dictionary<int, (int, int)>();

    private int _score;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            //Slider effect
            //textEffect

            CheckScoreAgainstLevel(_score);
            var (min, max) = ProgressDictionary[Level];
            ProgressLabel.text = $"{_score}/{max}";
        }
    }

    public int Level = 1;

    bool lvlUp = false;
    public void CheckScoreAgainstLevel(int score)
    {
        var (min, max) = ProgressDictionary[Level];
        if (score % 100 == 0 && Random.Range(0f, 1f) >= 0.5f) GameManager.Instance.GenerateShip();
        if (score >= max)
        {
            Level++;
            LevelLabel.text = $"lvl {Level}";
            //LevelEffect
            var (_min, _max) = ProgressDictionary[Level];
            ProgressSlider.minValue = _min;
            ProgressSlider.maxValue = _max;
            ProgressSlider.DOValue(score, 0.5f);
            CheckUnlocks();
        }
        else
        {
            ProgressSlider.DOValue(score, 0.5f);

        }

    }

    public void CheckUnlocks()
    {
        foreach (var skill in GameManager.Instance.SkillList)
        {
            if (skill.unlockLevel <= Level)
            {

                InterfaceManager.Instance.ShowUnlockPanel(skill.ID);
            }
        }

    }

}
