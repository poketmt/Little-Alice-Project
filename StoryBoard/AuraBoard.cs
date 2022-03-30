using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aura2API;

public enum AuraState
{
    Camera,
    Light,
    Volume
}

public class AuraBoard : StoryBoard
{
    [Header("Aura Board")]
    public AuraState auraState;

    public GameObject auraFirstComponent;       // Aura 옵션값 조절을 위한 첫번째 컴포넌트 대상 오브젝트
    public GameObject auraSecondComponent;      // Aura 옵션값 조절을 위한 두번째 컴포넌트 대상 오브젝트

    public AuraBaseSettings targetAuraSetting;  // Aura 옵션값 변화시킬 때 타겟이 될 대상 오라세팅 값 지정용

    public override void Start()
    {
        base.Start();
    }

    public override void RunStory()
    {
        base.RunStory();
        AuraManager.Instance.endAction = EndStory;

        switch (auraState)
        {
            case AuraState.Camera:
                AuraCamera mainAuraCamera = auraFirstComponent.GetComponent<AuraCamera>();
                AuraManager.Instance.AuraCameraSet(mainAuraCamera, targetAuraSetting);
                break;
            case AuraState.Light:
                AuraLight mainAuraLight = auraFirstComponent.GetComponent<AuraLight>();
                AuraLight targetAuraLight = auraSecondComponent.GetComponent<AuraLight>();
                AuraManager.Instance.AuraLightSet(mainAuraLight, targetAuraLight);
                break;
            case AuraState.Volume:
                AuraVolume mainAuraVolume = auraFirstComponent.GetComponent<AuraVolume>();
                AuraVolume targetAuraVolume = auraSecondComponent.GetComponent<AuraVolume>();
                AuraManager.Instance.AuraVolumeSet(mainAuraVolume, targetAuraVolume);
                break;

        }
    }

    public override void EndStory(float time)
    {
        base.EndStory(time);
    }
}
