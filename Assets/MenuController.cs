using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
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

    public SpriteRenderer ship;


    Color ScienceShipColor = new Color(1f, 1f, 1f, 1f);
    Color EngineerColor = new Color(1f, 0.5f, 0f, 1f);
    Color MilitaryColor = new Color(1f, 0f, 0f, 1f);
    Color PassengerColor = new Color(0f, 0.3f, 1f, 1f);
    private void OnEnable()
    {
        StartCoroutine(PlanetRoutine());
        hasCompletedTutorial = PlayerPrefs.GetInt("_tutorial", 0) == 0 ? false : true;

        if (!hasCompletedTutorial)
        {
            gameObject.SetActive(false);
            Tutorial1.SetActive(true);
            return;
        }
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
                ship.GetComponentInChildren<SpriteRenderer>().color = ScienceShipColor;

                break;
            case 1:
                ship.GetComponentInChildren<SpriteRenderer>().color = EngineerColor;

                break;
            case 2:
                ship.GetComponentInChildren<SpriteRenderer>().color = MilitaryColor;

                break;
            case 3:
                ship.GetComponentInChildren<SpriteRenderer>().color = PassengerColor;

                break;
        }
    }

    IEnumerator PlanetRoutine()
    {
        yield return null;
        int x = Random.Range(0, GameManager.Instance.planetList.Count);
        planet.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.planetList[x];
    }

    private void OnDisable()
    {
        SfxSlider.onValueChanged.RemoveAllListeners();
        MusicSlider.onValueChanged.RemoveAllListeners();
    }


    public void onStartButtonPressed()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StartGame();
        AudioManager.PlaySound("click");
    }
    public void onTutorialButtonPressed()
    {
        AudioManager.PlaySound("click");

        gameObject.SetActive(false);
        Tutorial1.SetActive(true);
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
