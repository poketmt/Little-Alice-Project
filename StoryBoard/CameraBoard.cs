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

    public GameObject targetPosition;           // Ÿ�� ������ ���� ������ ������Ʈ
    public GameObject mainCamera;               // ���� ī�޶� ������Ʈ
    public GameObject targetCamera;             // Ÿ�� ī�޶� ������Ʈ
    public GameObject characterTextCanvas;      // ĵ���� ������ ������Ʈ
    public float cameraSpeed;                   // ī�޶� �̵��ӵ� ������ ����
    public float shakeAmount;                   // ī�޶� ��鸮�� ũ�� ������ ����
    public float shakeSpeed;                    // ī�޶� ��鸮�� �ð� ������ ����

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
