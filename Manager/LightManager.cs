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

    private GameObject mainLight;           // ���� ����Ʈ ������
    private GameObject targetRotation;      // ����Ʈ ȸ����ų �� ���� �����̼� �� �����
    public Material skybox;                 // ��ī�̹ڽ� ���׸��� ������

    public float originExposure;            // ��ī�̹ڽ� ���׸��� ���� exposure�� ����� 
    private float targetExposure;           // ��ī�̹ڽ� ���׸��� ���� exposure�� �����
    private bool skyCheck = false;          // Update�� ��ī�̹ڽ� ����� �Ұ�
    private bool lightAction = false;       // Update�� ����Ʈ�׼� ����� �Ұ�
    private float rotationSpeed = 1f;       // ȸ�� �ӵ�. ����Ʈ�׼ǿ� ����Ʈ ȸ���ӵ� ������
    private float skyAnimTime;              // ��ī�̹ڽ� exposure�� ������ �� �ӵ� ������
    private float runtime;                  // �� ��� ����ð� ������
    private float time = 0;                 // ����ð� ����

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

            time += Time.deltaTime;     // �ð� ��� üũ �뵵

            if (time >= runtime)
            {
                mainLight.transform.rotation = targetRotation.transform.rotation;
                lightAction = false;
            }
            endAction(0f);
        }

        if (skyCheck)
        {
            // skybox�� color���� targetColor ��ǥ�� skyAnimTime * Time.deltaTime��ŭ�� �߰����� ���Ͽ� color�� ����
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

    // ������ ����Ǿ��� �� �����Ǵ� ������ �����Ű�� �޼���
    // �÷��� ���� �Ŀ� ��ī�̹ڽ� ���׸����� ���� ���� ������ �ǵ����� ���� ����
    void OnApplicationQuit()
    {
        if (skybox != null)
        {
            skybox.SetFloat("_Exposure", originExposure);
        }
    }

    // Light�� Intensity���� ��ȭ�� �� �� ���
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

    // Ư�� ��ġ�� ����Ʈ ����Ʈ �� �� ���
    public void SpotLightOn(Light light)
    {
        light.enabled = true;
        AudioManager.Instance.PlayOnetime("onSpot", 1f);
    }

    // Ȱ��ȭ ���� Light�� ���� �� �� ���
    public void LightOff(Light light)
    {
        light.enabled = false;
        AudioManager.Instance.PlayOnetime("OffSpot", 1f);
    }

    // ���� ���� �� �� ���(���� ȸ���� ���� �㳷 �ٲٱ� ��) 
    public void LightMoving(GameObject mainLight, GameObject targetRotation, float runtime)
    {
        lightAction = true;
        this.mainLight = mainLight;
        this.targetRotation = targetRotation;
        this.runtime = runtime;
    }

    // ��ī�̹ڽ� ���׸����� ��ü�� �� ���(��� ȯ�� ��ȯ ��)
    public void SkyboxChange(Material skyMat)
    {
        this.skybox = skyMat;
    }

    // ��ī�̹ڽ��� exposure���� ���� �� �� ���(ȯ������ ��� �� ������ �� ���� ���)
    public void SkyboxControl(float targetExposure, float skyAnimTime, float skyRuntime)
    {
        skyCheck = true;

        this.targetExposure = targetExposure;
        this.skyAnimTime = skyAnimTime;
        this.runtime = skyRuntime;
    }
}
