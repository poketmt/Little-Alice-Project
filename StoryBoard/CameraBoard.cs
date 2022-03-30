using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    Zoom,
    Change,
    ChangeWithBloom,
    Shake,
    Bounce
}

public class CameraBoard : StoryBoard
{
    [Header("Camera Board")]
    public CameraState cameraState;

    public GameObject targetPosition;           // 타겟 포지션 값을 지정할 오브젝트
    public GameObject mainCamera;               // 메인 카메라 오브젝트
    public GameObject targetCamera;             // 타겟 카메라 오브젝트
    public GameObject characterTextCanvas;      // 캔버스 지정용 오브젝트
    public float cameraSpeed;                   // 카메라 이동속도 지정용 변수
    public float shakeAmount;                   // 카메라 흔들리는 크기 지정용 변수
    public float shakeSpeed;                    // 카메라 흔들리는 시간 지정용 변수

    public override void Start()
    {
        base.Start();
    }

    public override void RunStory()
    {
        base.RunStory();

        CameraManager.instance.endAction = EndStory;
        switch (cameraState)
        {
            case CameraState.Zoom:
                CameraManager.instance.ActiveZoom(mainCamera, targetPosition, cameraSpeed);
                break;
            case CameraState.Change:
                CameraManager.instance.ChangeCamera(mainCamera, targetCamera, characterTextCanvas);
                break;
            case CameraState.ChangeWithBloom:
                CameraManager.instance.ChangeWithBloomCamera(mainCamera, targetCamera, characterTextCanvas);
                break;
            case CameraState.Shake:
                CameraManager.instance.ShakeCamera(mainCamera, shakeAmount, shakeSpeed);
                break;
            case CameraState.Bounce:
                CameraManager.instance.BounceActive(mainCamera);
                break;
        }
    }

    public override void EndStory(float time)
    {
        base.EndStory(time);
    }
}
