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
    public SmoothLocomotion locomotion;     // 플레이어 컨트롤러의 SmoothLocomotion 기능 호출을 위한 객체 생성

    private GameObject mainCamera;          // 현재 카메라 오브젝트
    private GameObject targetPosition;      // 타겟 위치 값 저장용 오브젝트

    public PostProcessVolume p_Volume;      // 포스트프로세스볼륨 지정
    Bloom bloom;                            // 블룸옵션 조절용

    Vector3 cameraOffset = new Vector3(0, 0.11f, 0.38f);    // 캔버스 종속위치 지정용 오프셋

    public Action<float> endAction;

    public GameObject CinematicLine;       // 시네마틱 라인 
    private float cameraSpeed;             // 카메라 속도 값 
    private float shakeAmount;             // 카메라 흔들리는 양
    private float shakeSpeed;              // 카메라 흔들리는 속도
    public bool zoomActive = false;        // 카메라 줌 시 불값
    public bool shakeActive = false;       // 카메라 쉐이크 시 불값
    public bool bounceActive = false;      // 카메라 바운스 시 불값

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

    // 지정위치로 카메라 줌 인/아웃 작동
    public void ActiveZoom(GameObject mainCamera, GameObject targetPosition, float cameraSpeed)
    {
        CinematicLine.SetActive(true);
        zoomActive = true;

        this.mainCamera = mainCamera;
        this.targetPosition = targetPosition;
        this.cameraSpeed = cameraSpeed;
    }

    // 현재 카메라를 지정 카메라로 전환
    public void ChangeCamera(GameObject mainCamera, GameObject targetCamera, GameObject textCanvas)
    {
        CinematicLine.SetActive(true);
        targetCamera.SetActive(true);
        mainCamera.SetActive(false);
        textCanvas.transform.parent = targetCamera.transform;
        endAction(1f);
    }

    // 지정카메라로 블룸효과 넣으며 전환
    public void ChangeWithBloomCamera(GameObject mainCamera, GameObject targetCamera, GameObject textCanvas)
    {
        p_Volume.profile.TryGetSettings(out bloom);
        bloom.intensity.value = 0;

        StartCoroutine(BloomCamera(mainCamera, targetCamera, textCanvas));
    }

    // 카메라 블룸 효과 작동
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

    // 카메라 쉐이킹(흔들림) 효과
    public void ShakeCamera(GameObject mainCamera, float shakeAmount, float shakeSpeed)
    {
        shakeActive = true;

        this.mainCamera = mainCamera;
        this.shakeAmount = shakeAmount;
        this.shakeSpeed = shakeSpeed;
    }

    // Update의 카메라 바운스(점프) 기능을 작동
    public void BounceActive(GameObject mainCamera)
    {
        bounceActive = true;
        this.mainCamera = mainCamera;
    }

    // 바운스 기능 호출
    public void BounceCamera()
    {
        locomotion.DoRigidBodyJump();
    }
}
