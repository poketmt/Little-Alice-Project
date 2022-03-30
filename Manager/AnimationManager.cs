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

    public GameObject CinematicLine;        // �ִϸ��̼� ��� �� ī�޶� �ó׸�ƽ���� �߰�
    // �ִϸ��̼� ��� �� ��� ��Ʈ�ѷ� ��Ȱ��ȭ �뵵
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

    // Ÿ�� �ִϸ��̼��� Ŭ�� ����(string)�� ���� ����
    public void AnimPlay(Animation animation, AnimationClip animClip, string callAnim)
    {
        animClip.legacy = true;
        animation.enabled = true;
        animation.Play(callAnim);
        StartCoroutine(OnFrameAnimationCheck(animation, animClip, callAnim));
    }

    // Ÿ�� �ִϸ������� �ִϸ��̼��� �Ķ���� ����(string)�� ���� ����
    public void AnimTrigger(Animator animator, string anim)
    {
        animator.enabled = true;
        animator.SetTrigger(anim);
        StartCoroutine(OnFrameAnimatorCheck(animator, anim));
    }

    // endAction�� �ִϸ��̼� ���� �� �Ͼ �� �ֵ��� �ϴ� ���(�ִϸ��̼����� ������ ��)
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

    // endAction�� �ִϸ��̼� ���� �� �Ͼ �� �ֵ��� �ϴ� ���(�ִϸ����ͷ� ������ ��)
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
