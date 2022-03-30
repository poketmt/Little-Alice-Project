using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoard : StoryBoard
{
    [Multiline(2)]
    public string Info;

    public enum AudioState
    {
        Loop,                // �����Ǵ� �� ����� ���
        Onetime,             // �ѹ��� ����Ǵ� ����� ���
        SmoothOff,           // ���� �������� ����� �������� ���߸鼭 ����
        Switch               // ���� ������� �����鼭 �� ����� ���
    }

    public AudioState audioState;

    public string loopAudio;        // Loop�� ����� Ŭ�� �̸�
    public string onetimeAudio;     // Onetime�� ����� Ŭ�� �̸�
    public float sfxVolume;         // ȿ���� ����ũ��
    public float delayTime;         // EndStory�� �����ð�
    public bool smoothOn = false;   // ���� ������ �ø� �� üũ

    public override void Start()
    {
        base.Start();
    }

    public override void RunStory()
    {
        base.RunStory();
        AudioManager.Instance.endAction = EndStory;
        switch (audioState)
        {
            case AudioState.Loop:
                AudioManager.Instance.PlayLoop(loopAudio, smoothOn);
                break;
            case AudioState.Onetime:
                AudioManager.Instance.PlayOnetime(onetimeAudio, sfxVolume);
                break;
            case AudioState.SmoothOff:
                AudioManager.Instance.StopBGM();
                break;
            case AudioState.Switch:
                AudioManager.Instance.AudioSwitch(loopAudio, smoothOn);
                break;
        }
    }

    public override void EndStory(float time)
    {
        time = delayTime;
        base.EndStory(time);
    }
}
