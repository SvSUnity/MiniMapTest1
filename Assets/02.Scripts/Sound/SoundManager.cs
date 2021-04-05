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
    public Slider BgmSl;
    public Slider EffectSl;

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
        audio.volume = BgmSl.value;
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

    public void PlayEffect(AudioClip sfx, GameObject go)
    {

        AudioSource _source;
        //뮤트상태면 재생X
        if (tg.isOn)
            return;

        //오디오 소스를 가진 오브젝트에서만 실행되도록 구성
        if (go.GetComponent<AudioSource>() != null)
            _source = go.GetComponent<AudioSource>();
        else
            return;
        //효과음반복재생목적
        if (_source.isPlaying)
            return;

        _source.clip = sfx;
        _source.volume = EffectSl.value;

        _source.Play();

    }
}
