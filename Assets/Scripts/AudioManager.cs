using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    static List<AudioClip> SFXList;
    static List<AudioClip> MusicList;

    static AudioSource SFXChannel;
    static AudioSource SFX2Channel;
    static AudioSource SFX3Channel;
    static AudioSource MusicChannel;

    private void Start()
    {
        Init();
    }

    void Init()
    {

        DOTween.Init();
        SFXChannel = GameObject.Find("SFXChannel").GetComponent<AudioSource>();
        SFX2Channel = GameObject.Find("SFX2Channel").GetComponent<AudioSource>();
        SFX3Channel = GameObject.Find("SFX3Channel").GetComponent<AudioSource>();
        MusicChannel = GameObject.Find("MusicChannel").GetComponent<AudioSource>();

        SFXList = new List<AudioClip>();
        MusicList = new List<AudioClip>();

        SFXList.AddRange(Resources.LoadAll<AudioClip>("SFX"));
        MusicList.AddRange(Resources.LoadAll<AudioClip>("Music"));

        Debug.Log($"AudioManager-> Init(); Loaded {SFXList.Count} sounds and {MusicList.Count} music tracks");
    }

    public static void PlaySound(string clipName)
    {

        AudioSource freeSource;
        if (SFXChannel.isPlaying)
        {
            if (SFX2Channel.isPlaying)
            {
                if (SFX3Channel.isPlaying)
                {
                    SFXChannel.Stop();
                    freeSource = SFXChannel;
                }
                else
                {
                    freeSource = SFX3Channel;
                }
            }
            else
            {
                freeSource = SFX2Channel;
            }
        }
        else
        {
            freeSource = SFXChannel;

        }

        foreach (var clip in SFXList)
        {
            if (clip.name == clipName)
            {
                freeSource.clip = clip;
                freeSource.Play();

            }
        }
    }
    public static void PlaySoundDelayed(string clipName, float delay)
    {
        Instance.StartCoroutine(DelayedSfxRoutine(clipName, delay));
    }

    static IEnumerator DelayedSfxRoutine(string clipname, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        PlaySound(clipname);
    }


    static float musicTime;
    public void ToggleMusic()
    {


        if (MusicChannel.isPlaying)
        {
            musicTime = MusicChannel.time;
            MusicChannel.Stop();
        }
        else
        {
            MusicChannel.time = musicTime;
            MusicChannel.Play();
        }
    }
}
