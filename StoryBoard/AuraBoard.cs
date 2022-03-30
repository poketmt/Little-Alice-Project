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

    public GameObject auraFirstComponent;       // Aura �ɼǰ� ������ ���� ù��° ������Ʈ ��� ������Ʈ
    public GameObject auraSecondComponent;      // Aura �ɼǰ� ������ ���� �ι�° ������Ʈ ��� ������Ʈ

    public AuraBaseSettings targetAuraSetting;  // Aura �ɼǰ� ��ȭ��ų �� Ÿ���� �� ��� ������ �� ������

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
