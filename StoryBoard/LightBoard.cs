using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LightsState
{
    Intensity,
    Skybox,
    EyeShadow,
    SpotOn,
    OffLight,
    Rotation,
}

public class LightBoard : StoryBoard
{
    [Header("Lights Board")]
    public LightsState lightsState;

    public GameObject mainLight;        // 메인 라이트 지정용
    public GameObject targetRot;        // 라이트 회전기능 사용 시 타겟 로테이션 값 지정용
    public Light targetLight;           // 타겟 라이트 지정용

    public float targetLightValue;      // 타겟 라이트 값 지정용
    public float targetExposure;        // 타겟 익스포저 값 지정용(스카이박스)
    public float setAnimTime;           // 스카이박스 기능 익스포저 값 변화할 속도 지정용
    public float setRuntime;            // 기능 재생시간 지정용

    public override void Start()
    {
        base.Start();
    }

    public override void RunStory()
    {
        base.RunStory();
        LightManager.Instance.endAction = EndStory;
        switch (lightsState)
        {
            case LightsState.Intensity:
                LightManager.Instance.IntensityControl(targetLight, targetLightValue);
                break;
            case LightsState.Skybox:
                LightManager.Instance.SkyboxControl(targetExposure, setAnimTime, setRuntime);
                break;
            case LightsState.SpotOn:
                LightManager.Instance.SpotLightOn(targetLight);
                break;
            case LightsState.OffLight:
                LightManager.Instance.LightOff(targetLight);
                break;
            case LightsState.Rotation:
                LightManager.Instance.LightMoving(mainLight, targetRot, setRuntime);
                break;
        }

    }
    public override void EndStory(float time)
    {
        base.EndStory(time);
    }
}
