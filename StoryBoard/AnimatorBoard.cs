using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimState
{
    AnimPlay,
    AnimTrigger,
    AnimBool
}

public class AnimatorBoard : StoryBoard
{
    public AnimState animState;

    public Animator targetAnimator;         // Ÿ�� �ִϸ����� ������ ����
    public Animation targetAnimation;       // Ÿ�� �ִϸ��̼� ������ ����
    public AnimationClip animClip;          // ����� �ִϸ��̼� Ŭ�� ������ ����

    public string callAnim;                 // ����� �ִϸ��̼��� �Ķ���� �Է¿� ����

    public override void Start()
    {
        base.Start();
    }

    public override void RunStory()
    {
        base.RunStory();
        AnimationManager.instance.endAction = EndStory;

        switch (animState)
        {
            case AnimState.AnimPlay:
                AnimationManager.instance.AnimPlay(targetAnimation, animClip, callAnim);
                break;
            case AnimState.AnimTrigger:
                AnimationManager.instance.AnimTrigger(targetAnimator, callAnim);
                break;

        }
    }

    public override void EndStory(float time)
    {
        base.EndStory(time);
    }
}
