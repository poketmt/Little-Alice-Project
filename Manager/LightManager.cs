using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aura2API;

public class LightManager : MonoBehaviour
{
    private static LightManager _instance;

    public static LightManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LightManager>();

            return _instance;
        }
    }

    public Action<float> endAction;

    private GameObject mainLight;           // 메인 라이트 지정용
    private GameObject targetRotation;      // 라이트 회전시킬 때 지정 로테이션 값 저장용
    public Material skybox;                 // 스카이박스 메테리얼 지정용

    public float originExposure;            // 스카이박스 메테리얼 원본 exposure값 저장용 
    private float targetExposure;           // 스카이박스 메테리얼 지정 exposure값 저장용
    private bool skyCheck = false;          // Update문 스카이박스 실행용 불값
    private bool lightAction = false;       // Update문 라이트액션 실행용 불값
    private float rotationSpeed = 1f;       // 회전 속도. 라이트액션에 라이트 회전속도 지정용
    private float skyAnimTime;              // 스카이박스 exposure값 조절할 때 속도 조절용
    private float runtime;                  // 각 기능 재생시간 지정용
    private float time = 0;                 // 실행시간 계산용

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<LightManager>();
        }
        if (_instance != this)
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (lightAction)
        {
            mainLight.transform.rotation = Quaternion.Slerp(mainLight.transform.rotation, targetRotation.transform.rotation, Time.deltaTime * rotationSpeed);

            time += Time.deltaTime;     // 시간 경과 체크 용도

            if (time >= runtime)
            {
                mainLight.transform.rotation = targetRotation.transform.rotation;
                lightAction = false;
            }
            endAction(0f);
        }

        if (skyCheck)
        {
            // skybox의 color부터 targetColor 목표중 skyAnimTime * Time.deltaTime만큼의 중간값을 구하여 color에 저장
            float exposure = Mathf.Lerp(skybox.GetFloat("_Exposure"), targetExposure, skyAnimTime * Time.deltaTime);
            skybox.SetFloat("_Exposure", exposure);

            time += Time.deltaTime;

            if (time >= runtime)
            {
                skybox.SetFloat("_Exposure", targetExposure);
                endAction(0f);
                skyCheck = false;
                time = 0;
            }
            DynamicGI.UpdateEnvironment();
        }
    }

    // 게임이 종료되었을 때 구현되는 내용을 실행시키는 메서드
    // 플레이 종료 후에 스카이박스 머테리얼의 값을 원래 값으로 되돌리기 위한 목적
    void OnApplicationQuit()
    {
        if (skybox != null)
        {
            skybox.SetFloat("_Exposure", originExposure);
        }
    }

    // Light의 Intensity값에 변화를 줄 때 사용
    public void IntensityControl(Light light, float targetLightValue)
    {
        if (light.intensity < targetLightValue)
        {
            StartCoroutine(LightIntensityUp(light, targetLightValue));
        }
        if (light.intensity > targetLightValue)
        {
            StartCoroutine(LightIntensityDown(light, targetLightValue));
        }
        
    }

    IEnumerator LightIntensityUp(Light light, float targetLightValue)
    {
        while (light.intensity <= targetLightValue)
        {
            light.intensity += 0.02f;
            yield return new WaitForSeconds(0.3f);
        }
        endAction(0f);
    }

    IEnumerator LightIntensityDown(Light light, float targetLightValue)
    {
        while (light.intensity >= targetLightValue)
        {
            light.intensity -= 0.1f;
            yield return new WaitForSeconds(0.5f);
        }
        endAction(0f);
    }

    // 특정 위치에 스포트 라이트 켤 때 사용
    public void SpotLightOn(Light light)
    {
        light.enabled = true;
        AudioManager.Instance.PlayOnetime("onSpot", 1f);
    }

    // 활성화 중인 Light를 꺼야 할 때 사용
    public void LightOff(Light light)
    {
        light.enabled = false;
        AudioManager.Instance.PlayOnetime("OffSpot", 1f);
    }

    // 조명 연출 할 때 사용(조명 회전을 통해 밤낮 바꾸기 등) 
    public void LightMoving(GameObject mainLight, GameObject targetRotation, float runtime)
    {
        lightAction = true;
        this.mainLight = mainLight;
        this.targetRotation = targetRotation;
        this.runtime = runtime;
    }

    // 스카이박스 메테리얼을 교체할 때 사용(장소 환경 전환 등)
    public void SkyboxChange(Material skyMat)
    {
        this.skybox = skyMat;
    }

    // 스카이박스의 exposure값을 조절 할 때 사용(환경조명 밝기 등 조절할 때 같이 사용)
    public void SkyboxControl(float targetExposure, float skyAnimTime, float skyRuntime)
    {
        skyCheck = true;

        this.targetExposure = targetExposure;
        this.skyAnimTime = skyAnimTime;
        this.runtime = skyRuntime;
    }
}
