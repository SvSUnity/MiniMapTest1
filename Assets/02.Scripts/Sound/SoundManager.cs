using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum BGM
{
    LOGIN,LOBBY,DAY,NIGHT
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    //싱글톤
    private static SoundManager _instance = null;

    //배경음리스트
    public AudioClip[] bgmList;

    AudioSource audio;

    //사운드바
    public Slider sl;

    //뮤트버튼
    public Toggle tg;

    //사운드패널
    public GameObject Sound;

    //사운드버튼
    public GameObject SoundBtn;

    
    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("SoundMananager == null");
            return _instance;
        }

    }

    void Awake()
    {
        _instance = this;
        audio = this.GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SoundBtn.SetActive(true);
        AudioSet();
    }
    public void AudioSet()
    {
        //AudioSource의 볼륨 셋팅 
        audio.volume = sl.value;
        //AudioSource의 Mute 셋팅 
        audio.mute = tg.isOn;
    }

    public void PlayBGM(int stage)
    {
        switch((BGM)stage)
        {
            case BGM.LOGIN:
                audio.clip = bgmList[(int)BGM.LOGIN];
                AudioSet();
                audio.Play();
                break;
            case BGM.LOBBY:
                audio.clip = bgmList[(int)BGM.LOBBY];
                AudioSet();
                audio.Play();
                break;
            case BGM.DAY:
                audio.clip = bgmList[(int)BGM.DAY];
                AudioSet();
                audio.Play();
                break;
            case BGM.NIGHT:
                audio.clip = bgmList[(int)BGM.NIGHT];
                AudioSet();
                audio.Play();
                break;

        }


    }
    public void SoundUiOpen()
    {
        Sound.SetActive(true);
        SoundBtn.SetActive(false);
    }

    public void SoundUiClose()
    {
        Sound.SetActive(false);
        SoundBtn.SetActive(true);
    }
}
