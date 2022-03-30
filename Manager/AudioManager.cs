using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AudioManager>();

            return _instance;
        }
    }

    [SerializeField] AudioClip bgmClip;          // 배경음 클립 참조시킬 변수
    [SerializeField] AudioClip[] audioClip;      // 오디오 클립들 참조시킬 배열 변수

    Dictionary<string, AudioClip> audioClipsDic; // 딕셔너리용 변수 생성      
    AudioSource sfxPlayer;                       // 효과음 재생용 오디오 소스
    AudioSource bgmPlayer;                       // 배경음 재생용 오디오 소스 

    public float masterVolumeSFX = 1f;           // 효과음 볼륨
    public float masterVolumeBGM = 1f;           // 배경음 볼륨

    public Action<float> endAction;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<AudioManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this) Destroy(gameObject);
        }
        InitAudioSetting();
    }

    // 초기 오디오 세팅작업
    void InitAudioSetting()
    {
        sfxPlayer = GetComponent<AudioSource>();
        SetupBGM();

        audioClipsDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip a in audioClip)
        {
            audioClipsDic.Add(a.name, a);
        }
    }

    // BGM플레이어를 생성하고, 기본설정을 초기화
    void SetupBGM()
    {
        GameObject child = new GameObject("BGMPlayer");
        child.transform.SetParent(transform);
        bgmPlayer = child.AddComponent<AudioSource>();
        bgmPlayer.clip = bgmClip;
        bgmPlayer.volume = masterVolumeBGM;
        bgmPlayer.loop = true;
    }

    private void Start()
    {
        if (bgmPlayer != null && bgmClip != null)
            bgmPlayer.Play();
    }

    // 오디오 한 번만 재생 할 때
    public void PlayOnetime(string a_name, float a_volume)
    {
        if (audioClipsDic[a_name] == false) 
            return;

        sfxPlayer.clip = audioClipsDic[a_name];
        sfxPlayer.PlayOneShot(audioClipsDic[a_name], a_volume * masterVolumeSFX);

        if (endAction != null)
            endAction(0f);
    }

    // 반복재생 해야하는 오디오 사용 시(주로 새 bgm 재생 시)
    public void PlayLoop(string a_name, bool smoothOn)
    {
        if (audioClipsDic[a_name] == false)
            return;

        bgmPlayer.Stop();
        if (smoothOn == true)
        {
            StartCoroutine(IncreaseVolume());
        }
        bgmPlayer.volume = masterVolumeBGM;
        bgmPlayer.clip = audioClipsDic[a_name];
        bgmPlayer.Play();

        if(endAction != null) 
           endAction(0f);
    }

    // 볼륨을 서서히 증가
    IEnumerator IncreaseVolume()
    {
        while (bgmPlayer.volume < 0.3)
        {
            bgmPlayer.volume += 0.02f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // 현재 재생중인 bgmPlayer의 음악을 정지
    public void StopBGM()
    {
        if (bgmPlayer.isPlaying && bgmPlayer.volume >= 0)
        {
            StartCoroutine(ReduceVolume());
        }

    }

    // 볼륨을 서서히 감소
    IEnumerator ReduceVolume()
    {
        while (bgmPlayer.volume > 0)
        {
            bgmPlayer.volume -= 0.02f;
            yield return new WaitForSeconds(0.2f);
        }
        bgmPlayer.Stop();
        if(endAction != null)
            endAction(0f);
    }

    // 볼륨을 서서히 줄이면서 교체할 때 사용, smoothOn을 true로 체크할 경우 볼륨이 서서히 증가
    public void AudioSwitch(string audio_Name, bool smoothOn)
    {
        if (audio_Name != null && bgmPlayer.volume >= 0)
        {
            StartCoroutine(ReduceVolumeForSwitch(audio_Name, smoothOn));
        }
    }

    // 볼륨을 서서히 줄이면서 중단하고, 다음 BGM을 재생. AudioSwitch에 사용
    IEnumerator ReduceVolumeForSwitch(string audio_Name, bool smoothOn)
    {
        while (bgmPlayer.volume > 0)
        {
            bgmPlayer.volume -= 0.02f;
            yield return new WaitForSeconds(0.2f);
        }
        bgmPlayer.Stop();
        if (!bgmPlayer.isPlaying)
        {
            PlayLoop(audio_Name, smoothOn);
        }
    }
}
