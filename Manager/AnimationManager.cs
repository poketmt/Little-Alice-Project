using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static AnimationManager _instance;

    public static AnimationManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AnimationManager>();
            }

            return _instance;
        }
    }

    public Action<float> endAction;

    public GameObject CinematicLine;        // 애니메이션 재생 시 카메라에 시네마틱라인 추가
    // 애니메이션 재생 시 양손 컨트롤러 비활성화 용도
    public GameObject h_Right;
    public GameObject h_Left;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<AnimationManager>();
        }
        if (_instance != this)
        {
            Destroy(this);
        }
    }

    // 타겟 애니메이션을 클립 네임(string)을 통해 실행
    public void AnimPlay(Animation animation, AnimationClip animClip, string callAnim)
    {
        animClip.legacy = true;
        animation.enabled = true;
        animation.Play(callAnim);
        StartCoroutine(OnFrameAnimationCheck(animation, animClip, callAnim));
    }

    // 타겟 애니메이터의 애니메이션을 파라미터 네임(string)을 통해 실행
    public void AnimTrigger(Animator animator, string anim)
    {
        animator.enabled = true;
        animator.SetTrigger(anim);
        StartCoroutine(OnFrameAnimatorCheck(animator, anim));
    }

    // endAction이 애니메이션 종료 후 일어날 수 있도록 하는 기능(애니메이션으로 실행할 때)
    IEnumerator OnFrameAnimationCheck(Animation animation, AnimationClip animClip, string anim)
    {
        yield return new WaitForSeconds(0.5f);

        while (animation.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        animClip.legacy = false;
        if (endAction != null)
            endAction(0f);
    }

    // endAction이 애니메이션 종료 후 일어날 수 있도록 하는 기능(애니메이터로 실행할 때)
    IEnumerator OnFrameAnimatorCheck(Animator animator, string anim)
    {
        yield return new WaitForSeconds(0.5f);

        while (animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
        {
            yield return new WaitForEndOfFrame();
        }
        if (endAction != null)
            endAction(0f);
    }
}
