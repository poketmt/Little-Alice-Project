using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aura2API;

public class AuraManager : MonoBehaviour
{
    private static AuraManager _Instance;

    public static AuraManager Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = FindObjectOfType<AuraManager>();

            return _Instance;
        }
    }
    
    public Action<float> endAction;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = FindObjectOfType<AuraManager>();
        }
        if (_Instance != this)
        {
            Destroy(this);
        }
    }

    // 오라옵션들을 일괄적으로 활성화/비활성화 할 때 사용
    public void ActiveAura(AuraCamera targetAuraCam, AuraLight targetAuraLight, AuraVolume targetAuraVolume)
    {
        if (targetAuraCam.enabled == true || targetAuraLight.enabled == true || targetAuraVolume.enabled == true)
        {
            targetAuraCam.enabled = false;
            targetAuraLight.enabled = false;
            targetAuraVolume.enabled = false;
        }
        else if (targetAuraCam.enabled == false || targetAuraLight.enabled == false || targetAuraVolume.enabled == false)
        {
            targetAuraCam.enabled = true;
            targetAuraLight.enabled = true;
            targetAuraVolume.enabled = true;
        }
        endAction(0f);
    }

    // 현재 AuraCamera옵션 타겟 값으로 변경할 때 사용
    public void AuraCameraSet(AuraCamera mainAura, AuraBaseSettings targetAura)
    {
        StartCoroutine(AuraCameraDensity(mainAura, targetAura));
    }

    // AuraCamera옵션 중 Density값 변경용
    IEnumerator AuraCameraDensity(AuraCamera mainAura, AuraBaseSettings targetAura)
    {
        if (mainAura.frustumSettings.baseSettings.density >= targetAura.density)
        {
            while (mainAura.frustumSettings.baseSettings.density > targetAura.density)
            {
                mainAura.frustumSettings.baseSettings.density -= 0.03f;
                yield return new WaitForSeconds(0.01f);
            }
            endAction(0f);
        }

        else if (mainAura.frustumSettings.baseSettings.density <= targetAura.density)
        {
            while (mainAura.frustumSettings.baseSettings.density < targetAura.density)
            {
                mainAura.frustumSettings.baseSettings.density += 0.03f;
                yield return new WaitForSeconds(0.03f);
            }
            endAction(0f);
        }
    }

    // 현재 AuraLight옵션 타겟 값으로 변경할 때 사용
    public void AuraLightSet(AuraLight auraLight, AuraLight targetLight)
    {
        StartCoroutine(AuraLightStrength(auraLight, targetLight));
    }

    // AuraLight옵션 중 Strength값 변경용
    IEnumerator AuraLightStrength(AuraLight mainAuraLight, AuraLight targetAuraLight)
    {
        if (mainAuraLight.strength >= targetAuraLight.strength)
        {
            while (mainAuraLight.strength > targetAuraLight.strength)
            {
                mainAuraLight.strength -= 1.5f * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            endAction(0f);
        }
        else if (mainAuraLight.strength <= targetAuraLight.strength)
        {
            while (mainAuraLight.strength < targetAuraLight.strength)
            {
                mainAuraLight.strength += 1.5f * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
            endAction(0f);
        }
    }

    // AuraVolume옵션 타겟 값으로 변경할 때 사용
    public void AuraVolumeSet(AuraVolume auraVolume, AuraVolume targetVolume)
    {
        if (auraVolume.densityInjection.enable == true)
        {
            StartCoroutine(AuraVolumeDensity(auraVolume, targetVolume));
        }
        if (auraVolume.lightInjection.injectionParameters.enable == true)
        {
            StartCoroutine(AuraVolumeLight(auraVolume, targetVolume));
        }
    }

    // AuraVolume옵션 중 Density값 변경용
    IEnumerator AuraVolumeDensity(AuraVolume auraVolume, AuraVolume targetVolume)
    {
        if (auraVolume.densityInjection.strength >= targetVolume.densityInjection.strength)
        {
            while (auraVolume.densityInjection.strength > targetVolume.densityInjection.strength)
            {
                auraVolume.densityInjection.strength -= 1.6f * Time.deltaTime;
                yield return new WaitForSeconds(0.05f);
            }
            endAction(0f);
        }

        else if (auraVolume.densityInjection.strength <= targetVolume.densityInjection.strength)
        {
            while (auraVolume.densityInjection.strength < targetVolume.densityInjection.strength)
            {
                auraVolume.densityInjection.strength += 1.6f * Time.deltaTime;
                yield return new WaitForSeconds(0.05f);
            }
            endAction(0f);
        }
    }

    // AuraVolume옵션 중 Light값 변경용
    IEnumerator AuraVolumeLight(AuraVolume auraVolume, AuraVolume targetVolume)
    {
        if (auraVolume.lightInjection.injectionParameters.strength >= targetVolume.lightInjection.injectionParameters.strength)
        {
            while (auraVolume.lightInjection.injectionParameters.strength > targetVolume.lightInjection.injectionParameters.strength)
            {
                auraVolume.lightInjection.injectionParameters.strength -= 1.6f * Time.deltaTime;
                yield return new WaitForSeconds(0.15f);
            }
            endAction(0f);
        }

        else if (auraVolume.lightInjection.injectionParameters.strength <= targetVolume.lightInjection.injectionParameters.strength)
        {
            while (auraVolume.lightInjection.injectionParameters.strength < targetVolume.lightInjection.injectionParameters.strength)
            {
                auraVolume.lightInjection.injectionParameters.strength += 1.6f * Time.deltaTime;
                yield return new WaitForSeconds(0.15f);
            }
            endAction(0f);
        }
    }
}
