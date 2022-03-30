using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;
using BNG;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;

    public static CameraManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
            }

            return _instance;
        }
    }
    public SmoothLocomotion locomotion;     // �÷��̾� ��Ʈ�ѷ��� SmoothLocomotion ��� ȣ���� ���� ��ü ����

    private GameObject mainCamera;          // ���� ī�޶� ������Ʈ
    private GameObject targetPosition;      // Ÿ�� ��ġ �� ����� ������Ʈ

    public PostProcessVolume p_Volume;      // ����Ʈ���μ������� ����
    Bloom bloom;                            // ���ɼ� ������

    Vector3 cameraOffset = new Vector3(0, 0.11f, 0.38f);    // ĵ���� ������ġ ������ ������

    public Action<float> endAction;

    public GameObject CinematicLine;       // �ó׸�ƽ ���� 
    private float cameraSpeed;             // ī�޶� �ӵ� �� 
    private float shakeAmount;             // ī�޶� ��鸮�� ��
    private float shakeSpeed;              // ī�޶� ��鸮�� �ӵ�
    public bool zoomActive = false;        // ī�޶� �� �� �Ұ�
    public bool shakeActive = false;       // ī�޶� ����ũ �� �Ұ�
    public bool bounceActive = false;      // ī�޶� �ٿ �� �Ұ�

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<CameraManager>();
        }
        if (_instance != this)
        {
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        if (bounceActive == true)
        {
            BounceCamera();
            if (mainCamera.transform.position.y >= 10)
            {
                bounceActive = false;
            }
        }
    }
    private void Update()
    {
        if (zoomActive == true)
        {
            if (mainCamera && targetPosition)
            {
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPosition.transform.position, cameraSpeed * Time.deltaTime);
                if ((mainCamera.transform.position - targetPosition.transform.position).magnitude <= 1f)
                {
                    zoomActive = false;
                    CinematicLine.SetActive(false);
                    endAction(0f);
                }
            }
        }

        if (shakeActive == true)
        {
            if (shakeAmount > 0)
            {
                mainCamera.transform.position = UnityEngine.Random.insideUnitSphere * shakeAmount + mainCamera.transform.position;
                shakeAmount -= shakeSpeed * Time.deltaTime;
            }
            else
            {
                shakeAmount = 0f;
                shakeActive = false;
                if (endAction != null)
                    endAction(0f);
            }
        }
    }

    // ������ġ�� ī�޶� �� ��/�ƿ� �۵�
    public void ActiveZoom(GameObject mainCamera, GameObject targetPosition, float cameraSpeed)
    {
        CinematicLine.SetActive(true);
        zoomActive = true;

        this.mainCamera = mainCamera;
        this.targetPosition = targetPosition;
        this.cameraSpeed = cameraSpeed;
    }

    // ���� ī�޶� ���� ī�޶�� ��ȯ
    public void ChangeCamera(GameObject mainCamera, GameObject targetCamera, GameObject textCanvas)
    {
        CinematicLine.SetActive(true);
        targetCamera.SetActive(true);
        mainCamera.SetActive(false);
        textCanvas.transform.parent = targetCamera.transform;
        endAction(1f);
    }

    // ����ī�޶�� ���ȿ�� ������ ��ȯ
    public void ChangeWithBloomCamera(GameObject mainCamera, GameObject targetCamera, GameObject textCanvas)
    {
        p_Volume.profile.TryGetSettings(out bloom);
        bloom.intensity.value = 0;

        StartCoroutine(BloomCamera(mainCamera, targetCamera, textCanvas));
    }

    // ī�޶� ��� ȿ�� �۵�
    IEnumerator BloomCamera(GameObject mainCamera, GameObject targetCamera, GameObject textCanvas)
    {
        while (bloom.intensity.value < 26)
        {
            bloom.intensity.value += 1;
            yield return new WaitForSeconds(0.07f);
        }

        mainCamera.SetActive(false);
        targetCamera.SetActive(true);
        textCanvas.transform.parent = targetCamera.transform;
        textCanvas.transform.position = targetCamera.transform.position;
        textCanvas.transform.rotation = targetCamera.transform.rotation;
        textCanvas.transform.localPosition += cameraOffset;

        while (bloom.intensity.value > 0)
        {
            bloom.intensity.value -= 1;
            yield return new WaitForSeconds(0.07f);
        }
        endAction(0f);
    }

    // ī�޶� ����ŷ(��鸲) ȿ��
    public void ShakeCamera(GameObject mainCamera, float shakeAmount, float shakeSpeed)
    {
        shakeActive = true;

        this.mainCamera = mainCamera;
        this.shakeAmount = shakeAmount;
        this.shakeSpeed = shakeSpeed;
    }

    // Update�� ī�޶� �ٿ(����) ����� �۵�
    public void BounceActive(GameObject mainCamera)
    {
        bounceActive = true;
        this.mainCamera = mainCamera;
    }

    // �ٿ ��� ȣ��
    public void BounceCamera()
    {
        locomotion.DoRigidBodyJump();
    }
}
