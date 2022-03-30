using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBoard : StoryBoard
{
    [Multiline(2)]
    public string Info;

    public enum AudioState
    {
        Loop,                // 루프되는 새 오디오 재생
        Onetime,             // 한번만 실행되는 오디오 재생
        SmoothOff,           // 현재 실행중인 오디오 볼륨값을 낮추면서 정지
        Switch               // 기존 오디오가 꺼지면서 새 오디오 재생
    }

    public AudioState audioState;

    public string loopAudio;        // Loop용 오디오 클립 이름
    public string onetimeAudio;     // Onetime용 오디오 클립 이름
    public float sfxVolume;         // 효과음 볼륨크기
    public float delayTime;         // EndStory용 지연시간
    public bool smoothOn = false;   // 볼륨 서서히 올릴 때 체크

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
