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

    [SerializeField] AudioClip bgmClip;          // ����� Ŭ�� ������ų ����
    [SerializeField] AudioClip[] audioClip;      // ����� Ŭ���� ������ų �迭 ����

    Dictionary<string, AudioClip> audioClipsDic; // ��ųʸ��� ���� ����      
    AudioSource sfxPlayer;                       // ȿ���� ����� ����� �ҽ�
    AudioSource bgmPlayer;                       // ����� ����� ����� �ҽ� 

    public float masterVolumeSFX = 1f;           // ȿ���� ����
    public float masterVolumeBGM = 1f;           // ����� ����

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

    // �ʱ� ����� �����۾�
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

    // BGM�÷��̾ �����ϰ�, �⺻������ �ʱ�ȭ
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

    // ����� �� ���� ��� �� ��
    public void PlayOnetime(string a_name, float a_volume)
    {
        if (audioClipsDic[a_name] == false) 
            return;

        sfxPlayer.clip = audioClipsDic[a_name];
        sfxPlayer.PlayOneShot(audioClipsDic[a_name], a_volume * masterVolumeSFX);

        if (endAction != null)
            endAction(0f);
    }

    // �ݺ���� �ؾ��ϴ� ����� ��� ��(�ַ� �� bgm ��� ��)
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

    // ������ ������ ����
    IEnumerator IncreaseVolume()
    {
        while (bgmPlayer.volume < 0.3)
        {
            bgmPlayer.volume += 0.02f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // ���� ������� bgmPlayer�� ������ ����
    public void StopBGM()
    {
        if (bgmPlayer.isPlaying && bgmPlayer.volume >= 0)
        {
            StartCoroutine(ReduceVolume());
        }

    }

    // ������ ������ ����
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

    // ������ ������ ���̸鼭 ��ü�� �� ���, smoothOn�� true�� üũ�� ��� ������ ������ ����
    public void AudioSwitch(string audio_Name, bool smoothOn)
    {
        if (audio_Name != null && bgmPlayer.volume >= 0)
        {
            StartCoroutine(ReduceVolumeForSwitch(audio_Name, smoothOn));
        }
    }

    // ������ ������ ���̸鼭 �ߴ��ϰ�, ���� BGM�� ���. AudioSwitch�� ���
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
