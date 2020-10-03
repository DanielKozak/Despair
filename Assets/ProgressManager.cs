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
        ProgressDictionary.Add(1, (0, 10));
        ProgressDictionary.Add(2, (10, 50));
        ProgressDictionary.Add(3, (50, 150));
        ProgressDictionary.Add(4, (150, 500));
        ProgressDictionary.Add(5, (500, 1000));
        ProgressDictionary.Add(6, (1000, 5000));
        ProgressDictionary.Add(7, (5000, 10000));
        ProgressDictionary.Add(8, (10000, 20000));
        ProgressDictionary.Add(9, (20000, 50000));
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
        if (score >= max)
        {
            Level++;
            LevelLabel.text = $"lvl{Level}";
            //LevelEffect
            var (_min, _max) = ProgressDictionary[Level];
            ProgressSlider.minValue = _min;
            ProgressSlider.maxValue = _max;
            ProgressSlider.value = score;
        }
        else
        {
            ProgressSlider.value = score;

        }

    }

    public void CheckUnlocks()
    {
        foreach (var skill in GameManager.Instance.SkillList)
        {
            if (skill.unlockLevel == Level)
            {

                //PAUSEGAME

                //TODO Unlock Skill
            }
        }

    }

}
