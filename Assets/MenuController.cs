using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private EventChannelGameFlow _eventChannelGameFlow;
    [SerializeField] private EventChannelAudioMix _eventChannelAudioMix;
    [SerializeField] private EventChannelAudioControl _eventChannelAudioControl;

    [Space]
    public Text HighScoreText;
    public bool hasTorturedShip;

    public GameObject TorturedPanel;
    public Text TorturedShipName;
    public Text TorturedShipTime;

    public int TorturedShipType = -1;

    public bool hasCompletedTutorial;
    public GameObject Tutorial1;
    public GameObject Tutorial2;

    public AudioSource Sfx1;
    public AudioSource Sfx2;
    public AudioSource Sfx3;
    public AudioSource music;

    public Slider SfxSlider;
    public Slider MusicSlider;

    public GameObject planet;
    public GameObject tutPlanet;
    public Image ship;

    public Button StartButton;


    Color ScienceShipColor = new Color(1f, 1f, 1f, 1f);
    Color EngineerColor = new Color(1f, 0.5f, 0f, 1f);
    Color MilitaryColor = new Color(1f, 0f, 0f, 1f);
    Color PassengerColor = new Color(0f, 0.3f, 1f, 1f);


    private void OnEnable()
    {
        //Vector3 newPos = new Vector3(-16.9f, -2.6f, 0);
        //planet.transform.position = newPos;


        Debug.Log($"loading save data");
        Debug.Log($"_tutorial {PlayerPrefs.GetInt("_tutorial", 0)}");
        Debug.Log($"_highscore {PlayerPrefs.GetInt("_highscore", 0)}");
        Debug.Log($"_torturedName {PlayerPrefs.GetString("_torturedName", "")}");
        Debug.Log($"_torturedTime {PlayerPrefs.GetFloat("_torturedTime", 0)}");
        Debug.Log($"_torturedtype {PlayerPrefs.GetInt("_torturedtype", -1)}");

        hasCompletedTutorial = PlayerPrefs.GetInt("_tutorial", 0) == 0 ? false : true;


        planet.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.3f, 0.4f, 10f));
        tutPlanet.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.3f, 0.6f, 10f));

        if (!hasCompletedTutorial)
        {
            gameObject.SetActive(false);
            Tutorial1.SetActive(true);
            return;
        }
        Debug.Log($"loading save data");

        HighScoreText.text = PlayerPrefs.GetInt("_highscore", 0).ToString();
        TorturedShipName.text = PlayerPrefs.GetString("_torturedName", "");
        TorturedShipTime.text = PlayerPrefs.GetFloat("_torturedTime", 0).ToString();
        TorturedShipType = PlayerPrefs.GetInt("_torturedtype", -1);
        if (TorturedShipType == -1)
        {
            TorturedPanel.SetActive(false);
        }

        SfxSlider.value = Sfx1.volume;
        MusicSlider.value = music.volume;

        SfxSlider.onValueChanged.AddListener(delegate { onSFXChanged(); });
        MusicSlider.onValueChanged.AddListener(delegate { onMusicChanged(); });

        switch (TorturedShipType)
        {
            case 0:
                ship.color = ScienceShipColor;

                break;
            case 1:
                ship.color = EngineerColor;

                break;
            case 2:
                ship.color = MilitaryColor;

                break;
            case 3:
                ship.color = PassengerColor;

                break;
        }

        if (GameManager.Instance != null)
            if (GameManager.Instance.isFromMenu)
            {
                StartButton.enabled = false;
                StartButton.GetComponentInChildren<Text>().text = "Sorry, restart broken";
            }
    }

    private void OnDisable()
    {
        SfxSlider.onValueChanged.RemoveAllListeners();
        MusicSlider.onValueChanged.RemoveAllListeners();

    }

    public void onStartButtonPressed()
    {
        _eventChannelGameFlow.RaiseOnNewGameStartEvent();
        StartButton.gameObject.SetActive(false);
        // _eventChannelAudioControl
        AudioManager.PlaySound("click");
    }
    public void onTutorialButtonPressed()
    {
        AudioManager.PlaySound("click");

        // gameObject.SetActive(false);
        // Tutorial1.SetActive(true);
    }
    public void onExitButtonPressed()
    {
        AudioManager.PlaySound("click");

        Application.Quit();
    }

    public void onSFXChanged()
    {
        Sfx1.volume = Sfx2.volume = Sfx3.volume = SfxSlider.value;
    }
    public void onMusicChanged()
    {
        music.volume = MusicSlider.value;
    }

    public void OnTutorialDone()
    {
        PlayerPrefs.SetInt("_tutorial", 1);
    }

}