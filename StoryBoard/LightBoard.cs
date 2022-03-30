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

    public GameObject mainLight;        // ���� ����Ʈ ������
    public GameObject targetRot;        // ����Ʈ ȸ����� ��� �� Ÿ�� �����̼� �� ������
    public Light targetLight;           // Ÿ�� ����Ʈ ������

    public float targetLightValue;      // Ÿ�� ����Ʈ �� ������
    public float targetExposure;        // Ÿ�� �ͽ����� �� ������(��ī�̹ڽ�)
    public float setAnimTime;           // ��ī�̹ڽ� ��� �ͽ����� �� ��ȭ�� �ӵ� ������
    public float setRuntime;            // ��� ����ð� ������

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
